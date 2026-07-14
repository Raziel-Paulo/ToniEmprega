using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Text.Json;
using MailKit.Net.Smtp;
using MimeKit;
using ToniEmprega.Data;
using ToniEmprega.Models;

namespace ToniEmprega.Controllers
{
    public class AccountController : Controller
    {
        private const string PendingRegisterKey = "PendingRegisterData";
        private const string PendingRegisterCodeKey = "PendingRegisterCode";
        private const string PendingRegisterTimeKey = "PendingRegisterTime";

        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AccountController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        private sealed class PendingRegisterData
        {
            public string Nome { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string PasswordHash { get; set; } = string.Empty;
            public DateTime DataNascimento { get; set; }
            public int TipoUtilizadorId { get; set; }
            public string TipoDesignacao { get; set; } = string.Empty;
            public int? IdTurma { get; set; }
            public string? NumeroAluno { get; set; }
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            try
            {
                email = email?.Trim();

                var user = await _context.Utilizadores
                    .AsNoTracking()
                    .Include(u => u.TipoUtilizador)
                    .FirstOrDefaultAsync(u => u.Email == email);

                if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Palavra_Passe))
                {
                    ViewBag.Error = "Email ou palavra-passe incorretos.";
                    return View();
                }

                if (user.Bloqueado)
                {
                    ViewBag.Error = "A sua conta foi bloqueada pelo administrador. Contacte o suporte para mais informações.";
                    return View();
                }

                var userType = user.TipoUtilizador?.Designacao ?? string.Empty;

                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("UserName", user.Nome);
                HttpContext.Session.SetString("UserType", userType);

                if (user.Id_Estado_Validacao_Utilizador == 1)
                {
                    TempData["Warning"] = "Precisa de completar a validação de identidade.";
                    return RedirectToAction("Index", "Validacao");
                }

                if (user.Id_Estado_Validacao_Utilizador == 3)
                {
                    TempData["Error"] = "A sua validação foi rejeitada. Submeta novo documento.";
                    return RedirectToAction("Index", "Validacao");
                }

                _context.Notificacoes.Add(new Notificacao
                {
                    Id_Utilizador = user.Id,
                    Titulo = "Novo login",
                    Mensagem = $"Login realizado em {DateTime.UtcNow:dd/MM/yyyy HH:mm}",
                    Tipo = "info"
                });
                await _context.SaveChangesAsync();

                return userType switch
                {
                    "Aluno" => RedirectToAction("Dashboard", "Aluno"),
                    "Professor" => RedirectToAction("Dashboard", "Professor"),
                    "Empresa" => RedirectToAction("Dashboard", "Empresa"),
                    "Administrador" => RedirectToAction("Dashboard", "Admin"),
                    _ => RedirectToAction("Index", "Home")
                };
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        public async Task<IActionResult> Register()
        {
            await CarregarTiposUtilizadorAsync();
            ViewBag.Turmas = await _context.Turmas.OrderBy(t => t.Designacao).ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Utilizador utilizador, string confirmPassword, int? idTurma = null, string? numeroAluno = null)
        {
            await CarregarTiposUtilizadorAsync();
            ViewBag.Turmas = await _context.Turmas.OrderBy(t => t.Designacao).ToListAsync();

            utilizador.Nome = utilizador.Nome?.Trim();
            utilizador.Email = utilizador.Email?.Trim();

            if (utilizador.Id_Tipo_Utilizador <= 0)
            {
                ModelState.AddModelError(nameof(utilizador.Id_Tipo_Utilizador), "Tem de selecionar um tipo de utilizador.");
            }

            if (!IsPasswordComplex(utilizador.Palavra_Passe))
            {
                ModelState.AddModelError(string.Empty, "A palavra-passe deve ter pelo menos 8 caracteres, incluindo maiúsculas, minúsculas, números e caracteres especiais.");
            }

            if (utilizador.Palavra_Passe != confirmPassword)
            {
                ModelState.AddModelError(string.Empty, "As palavras-passe não coincidem.");
            }

            if (!IsValidEmail(utilizador.Email))
            {
                ModelState.AddModelError(string.Empty, "Email inválido. Use um formato válido (ex: nome@dominio.pt).");
            }

            if (await _context.Utilizadores.AnyAsync(u => u.Email == utilizador.Email))
            {
                ModelState.AddModelError(string.Empty, "Email já registado.");
            }

            if (!utilizador.Data_Nascimento.HasValue)
            {
                ModelState.AddModelError(string.Empty, "Data de nascimento é obrigatória.");
            }

            var tipo = await _context.TipoUtilizadores.FindAsync(utilizador.Id_Tipo_Utilizador);
            if (tipo == null || tipo.Designacao == "Administrador")
            {
                ModelState.AddModelError(string.Empty, "Tipo de utilizador inválido.");
            }

            if (tipo != null && tipo.Designacao == "Aluno" && (!idTurma.HasValue || idTurma.Value <= 0))
            {
                ModelState.AddModelError(string.Empty, "Tem de selecionar uma turma.");
            }

            if (tipo != null && tipo.Designacao == "Aluno" && string.IsNullOrWhiteSpace(numeroAluno))
            {
                ModelState.AddModelError(string.Empty, "Tem de indicar o número de aluno.");
            }

            if (tipo != null && tipo.Designacao == "Aluno" && !string.IsNullOrWhiteSpace(numeroAluno))
            {
                var numeroAlunoLimpo = numeroAluno.Trim();
                var numeroExiste = await _context.Alunos.AnyAsync(a => a.Numero_Aluno == numeroAlunoLimpo);
                if (numeroExiste)
                {
                    ModelState.AddModelError(string.Empty, "Já existe um aluno registado com este número de aluno. Por favor, use outro número.");
                }
            }

            if (ModelState.IsValid)
            {
                var idade = CalcularIdade(utilizador.Data_Nascimento!.Value);

                if (tipo!.Designacao == "Aluno" && idade < 17)
                {
                    ModelState.AddModelError(string.Empty, "Para se registar como Aluno, deve ter pelo menos 17 anos.");
                }
            }

            if (!ModelState.IsValid)
            {
                return View(utilizador);
            }

            var codigo = Random.Shared.Next(100000, 999999).ToString();
            var pending = new PendingRegisterData
            {
                Nome = utilizador.Nome.Trim(),
                Email = utilizador.Email.Trim(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(utilizador.Palavra_Passe),
                DataNascimento = utilizador.Data_Nascimento!.Value.Date,
                TipoUtilizadorId = utilizador.Id_Tipo_Utilizador,
                TipoDesignacao = tipo!.Designacao,
                IdTurma = idTurma,
                NumeroAluno = numeroAluno
            };

            HttpContext.Session.SetString(PendingRegisterKey, JsonSerializer.Serialize(pending));
            HttpContext.Session.SetString(PendingRegisterCodeKey, codigo);
            HttpContext.Session.SetString(PendingRegisterTimeKey, DateTime.UtcNow.ToString("O"));

            try
            {
                await EnviarEmailConfirmacaoRegisto(pending.Email, codigo, pending.Nome);
                TempData["Success"] = "Enviámos um código de confirmação para o seu email.";
            }
            catch (Exception ex)
            {
                TempData["Warning"] = $"Erro ao enviar email: {ex.Message}. O seu código é: {codigo}";
            }

            return RedirectToAction(nameof(VerificarEmailRegisto));
        }

        public IActionResult VerificarEmailRegisto()
        {
            if (HttpContext.Session.GetString(PendingRegisterKey) == null || HttpContext.Session.GetString(PendingRegisterCodeKey) == null)
                return RedirectToAction(nameof(Register));

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerificarEmailRegisto(string codigo)
        {
            var storedCode = HttpContext.Session.GetString(PendingRegisterCodeKey);
            var storedTime = HttpContext.Session.GetString(PendingRegisterTimeKey);
            var pendingJson = HttpContext.Session.GetString(PendingRegisterKey);

            if (string.IsNullOrWhiteSpace(storedCode) || string.IsNullOrWhiteSpace(storedTime) || string.IsNullOrWhiteSpace(pendingJson))
                return RedirectToAction(nameof(Register));

            if (!DateTime.TryParse(storedTime, null, DateTimeStyles.RoundtripKind, out var verificationTime) || DateTime.UtcNow > verificationTime.AddMinutes(15))
            {
                LimparSessaoRegistoPendente();
                TempData["Error"] = "O código expirou. Faça o registo novamente.";
                return RedirectToAction(nameof(Register));
            }

            if (!string.Equals(codigo?.Trim(), storedCode, StringComparison.Ordinal))
            {
                TempData["Error"] = "Código incorreto.";
                return View();
            }

            var pending = JsonSerializer.Deserialize<PendingRegisterData>(pendingJson);
            if (pending == null)
            {
                LimparSessaoRegistoPendente();
                TempData["Error"] = "Não foi possível recuperar os dados do registo. Faça o registo novamente.";
                return RedirectToAction(nameof(Register));
            }

            var utilizador = new Utilizador
            {
                Nome = pending.Nome,
                Email = pending.Email,
                Palavra_Passe = pending.PasswordHash,
                Data_Nascimento = DateTime.SpecifyKind(pending.DataNascimento.Date, DateTimeKind.Utc),
                Data_Registro = DateTime.UtcNow,
                Id_Tipo_Utilizador = pending.TipoUtilizadorId,
                Id_Estado_Validacao_Utilizador = 1
            };

            _context.Utilizadores.Add(utilizador);
            await _context.SaveChangesAsync();

            switch (pending.TipoDesignacao)
            {
                case "Aluno":
                    _context.Alunos.Add(new Aluno
                    {
                        Id_Utilizador = utilizador.Id,
                        Curso = string.Empty,
                        Ano_Letivo = string.Empty,
                        Numero_Aluno = pending.NumeroAluno ?? string.Empty,
                        Id_Turma = pending.IdTurma > 0 ? pending.IdTurma : null
                    });
                    break;
                case "Professor":
                    _context.Professores.Add(new Professor
                    {
                        Id_Utilizador = utilizador.Id,
                        Departamento = string.Empty,
                        Numero_Professor = string.Empty
                    });
                    break;
                case "Empresa":
                    _context.Empresas.Add(new Empresa
                    {
                        Id_Utilizador = utilizador.Id,
                        Nome_Empresa = utilizador.Nome,
                        Nif = string.Empty,
                        Morada = string.Empty,
                        Site_Empresa = string.Empty,
                        Telefone = string.Empty
                    });
                    break;
            }

            await _context.SaveChangesAsync();

            var admins = await _context.Admins.Include(a => a.Utilizador).ToListAsync();
            foreach (var admin in admins)
            {
                _context.Notificacoes.Add(new Notificacao
                {
                    Id_Utilizador = admin.Id_Utilizador,
                    Titulo = "Novo registo pendente",
                    Mensagem = $"{utilizador.Nome} ({pending.TipoDesignacao}) registou-se e aguarda validação de documento.",
                    Tipo = "warning",
                    Link = "/Admin/Validacoes"
                });
            }
            await _context.SaveChangesAsync();

            HttpContext.Session.SetInt32("UserId", utilizador.Id);
            HttpContext.Session.SetString("UserName", utilizador.Nome);
            HttpContext.Session.SetString("UserType", pending.TipoDesignacao);

            LimparSessaoRegistoPendente();

            TempData["Success"] = "Email confirmado! Agora submeta o seu documento de identificação.";
            return RedirectToAction("Index", "Validacao");
        }

        public IActionResult RecuperarPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RecuperarPassword(string email)
        {
            try
            {
                var user = await _context.Utilizadores
                    .FirstOrDefaultAsync(u => u.Email == email);

                if (user == null)
                {
                    TempData["Success"] = "Se o email existir no sistema, receberá um código de recuperação.";
                    return RedirectToAction(nameof(RecuperarPassword));
                }

                var codigo = Random.Shared.Next(100000, 999999).ToString();

                HttpContext.Session.SetString("ResetCode", codigo);
                HttpContext.Session.SetString("ResetEmail", email);
                HttpContext.Session.SetString("ResetTime", DateTime.UtcNow.ToString("O"));

                try
                {
                    await EnviarEmailRecuperacao(email, codigo, user.Nome);
                    TempData["Success"] = "Código de recuperação enviado para o seu email.";
                }
                catch (Exception ex)
                {
                    TempData["Warning"] = $"Erro ao enviar email: {ex.Message}. O seu código é: {codigo}";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }

            return RedirectToAction(nameof(VerificarCodigo));
        }

        public IActionResult VerificarCodigo()
        {
            if (HttpContext.Session.GetString("ResetCode") == null)
                return RedirectToAction(nameof(RecuperarPassword));

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult VerificarCodigo(string codigo)
        {
            var storedCode = HttpContext.Session.GetString("ResetCode");
            var resetTimeRaw = HttpContext.Session.GetString("ResetTime");

            if (string.IsNullOrWhiteSpace(storedCode) || string.IsNullOrWhiteSpace(resetTimeRaw) || !DateTime.TryParse(resetTimeRaw, null, DateTimeStyles.RoundtripKind, out var resetTime))
            {
                LimparSessaoRecuperacaoPassword();
                TempData["Error"] = "Sessão de recuperação inválida. Solicite um novo código.";
                return RedirectToAction(nameof(RecuperarPassword));
            }

            if (DateTime.UtcNow > resetTime.AddMinutes(15))
            {
                LimparSessaoRecuperacaoPassword();
                TempData["Error"] = "Código expirado. Solicite novo código.";
                return RedirectToAction(nameof(RecuperarPassword));
            }

            if (!string.Equals(codigo?.Trim(), storedCode, StringComparison.Ordinal))
            {
                TempData["Error"] = "Código incorreto.";
                return View();
            }

            return RedirectToAction(nameof(NovaPassword));
        }

        public IActionResult NovaPassword()
        {
            if (HttpContext.Session.GetString("ResetCode") == null)
                return RedirectToAction(nameof(RecuperarPassword));

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NovaPassword(string novaPassword, string confirmarPassword)
        {
            if (novaPassword != confirmarPassword)
            {
                TempData["Error"] = "As passwords não coincidem.";
                return View();
            }

            if (!IsPasswordComplex(novaPassword))
            {
                TempData["Error"] = "A palavra-passe deve ter pelo menos 8 caracteres, incluindo maiúsculas, minúsculas, números e caracteres especiais.";
                return View();
            }

            var email = HttpContext.Session.GetString("ResetEmail");
            var user = await _context.Utilizadores.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                LimparSessaoRecuperacaoPassword();
                return RedirectToAction(nameof(RecuperarPassword));
            }

            user.Palavra_Passe = BCrypt.Net.BCrypt.HashPassword(novaPassword);
            await _context.SaveChangesAsync();

            LimparSessaoRecuperacaoPassword();
            TempData["Success"] = "Password alterada com sucesso! Faça login com a nova password.";
            return RedirectToAction(nameof(Login));
        }

        private async Task CarregarTiposUtilizadorAsync()
        {
            ViewBag.TiposUtilizador = await _context.TipoUtilizadores
                .Where(t => t.Designacao != "Administrador")
                .OrderBy(t => t.Designacao)
                .ToListAsync();
        }

        private async Task EnviarEmailConfirmacaoRegisto(string email, string codigo, string nome)
        {
            var smtpServer = _configuration["Email:SmtpServer"] ?? "smtp.sendgrid.net";
            var smtpPort = int.Parse(_configuration["Email:SmtpPort"] ?? "587");
            var smtpUser = _configuration["Email:SmtpUser"] ?? "apikey";
            var smtpPass = _configuration["Email:SmtpPass"] ?? string.Empty;
            var fromEmail = _configuration["Email:FromEmail"] ?? "toniemprega@gmail.com";

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("ToniEmprega", fromEmail));
            message.To.Add(new MailboxAddress(nome, email));
            message.Subject = "Confirmação de Email";

            message.Body = new TextPart("html")
            {
                Text = $@"
<h2>Confirmação de Email - ToniEmprega</h2>
<p>Olá {nome},</p>
<p>Use este código para confirmar o seu registo:</p>
<h1 style='color: #1E90FF; font-size: 32px; letter-spacing: 5px;'>{codigo}</h1>
<p>Este código é válido por 15 minutos.</p>
<p>Se não iniciou este registo, ignore este email.</p>
<br>
<p>Atenciosamente,<br>Equipa ToniEmprega</p>"
            };

            using var client = new MailKit.Net.Smtp.SmtpClient();
            await client.ConnectAsync(smtpServer, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(smtpUser, smtpPass);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

        private async Task EnviarEmailRecuperacao(string email, string codigo, string nome)
        {
            var smtpServer = _configuration["Email:SmtpServer"] ?? "smtp.sendgrid.net";
            var smtpPort = int.Parse(_configuration["Email:SmtpPort"] ?? "587");
            var smtpUser = _configuration["Email:SmtpUser"] ?? "apikey";
            var smtpPass = _configuration["Email:SmtpPass"] ?? string.Empty;
            var fromEmail = _configuration["Email:FromEmail"] ?? "toniemprega@gmail.com";

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("ToniEmprega", fromEmail));
            message.To.Add(new MailboxAddress(nome, email));
            message.Subject = "Recuperação de Password";

            message.Body = new TextPart("html")
            {
                Text = $@"
<h2>Recuperação de Password - ToniEmprega</h2>
<p>Olá {nome},</p>
<p>Recebemos um pedido para recuperar a password da sua conta.</p>
<p>O seu código de verificação é:</p>
<h1 style='color: #1E90FF; font-size: 32px; letter-spacing: 5px;'>{codigo}</h1>
<p>Este código é válido por 15 minutos.</p>
<p>Se não solicitou esta recuperação, ignore este email.</p>
<br>
<p>Atenciosamente,<br>Equipa ToniEmprega</p>"
            };

            using var client = new MailKit.Net.Smtp.SmtpClient();
            await client.ConnectAsync(smtpServer, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(smtpUser, smtpPass);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

        private bool IsPasswordComplex(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
                return false;

            var hasUpper = password.Any(char.IsUpper);
            var hasLower = password.Any(char.IsLower);
            var hasDigit = password.Any(char.IsDigit);
            var hasSpecial = password.Any(c => !char.IsLetterOrDigit(c));

            return hasUpper && hasLower && hasDigit && hasSpecial;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email && email.Contains('.');
            }
            catch
            {
                return false;
            }
        }

        private int CalcularIdade(DateTime dataNascimento)
        {
            var hoje = DateTime.Today;
            var idade = hoje.Year - dataNascimento.Year;
            if (dataNascimento.Date > hoje.AddYears(-idade)) idade--;
            return idade;
        }

        private void LimparSessaoRegistoPendente()
        {
            HttpContext.Session.Remove(PendingRegisterKey);
            HttpContext.Session.Remove(PendingRegisterCodeKey);
            HttpContext.Session.Remove(PendingRegisterTimeKey);
        }

        private void LimparSessaoRecuperacaoPassword()
        {
            HttpContext.Session.Remove("ResetCode");
            HttpContext.Session.Remove("ResetEmail");
            HttpContext.Session.Remove("ResetTime");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Perfil()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue) return RedirectToAction(nameof(Login));

            var user = await _context.Utilizadores
                .Include(u => u.TipoUtilizador)
                .Include(u => u.EstadoValidacao)
                .FirstOrDefaultAsync(u => u.Id == userId.Value);

            if (user == null) return NotFound();

            ViewBag.NotificacoesNaoLidas = await _context.Notificacoes
                .CountAsync(n => n.Id_Utilizador == userId.Value && !n.Lida);

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AtualizarPerfil(string nome, DateTime? dataNascimento)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue) return RedirectToAction(nameof(Login));

            var user = await _context.Utilizadores.FindAsync(userId.Value);
            if (user == null) return NotFound();

            user.Nome = nome;
            user.Data_Nascimento = dataNascimento.HasValue ? DateTime.SpecifyKind(dataNascimento.Value.Date, DateTimeKind.Utc) : null;
            await _context.SaveChangesAsync();

            HttpContext.Session.SetString("UserName", user.Nome);
            TempData["Success"] = "Perfil atualizado com sucesso!";
            return RedirectToAction(nameof(Perfil));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AlterarPassword(string passwordAtual, string novaPassword, string confirmarPassword)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue) return RedirectToAction(nameof(Login));

            var user = await _context.Utilizadores.FindAsync(userId.Value);
            if (user == null) return NotFound();

            if (!BCrypt.Net.BCrypt.Verify(passwordAtual, user.Palavra_Passe))
            {
                TempData["Error"] = "Password atual incorreta.";
                return RedirectToAction(nameof(Perfil));
            }

            if (novaPassword != confirmarPassword)
            {
                TempData["Error"] = "As novas passwords não coincidem.";
                return RedirectToAction(nameof(Perfil));
            }

            user.Palavra_Passe = BCrypt.Net.BCrypt.HashPassword(novaPassword);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Password alterada com sucesso!";
            return RedirectToAction(nameof(Perfil));
        }
    }
}