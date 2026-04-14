// Controllers/AccountController.cs - COMPLETO COM VALIDAÇÃO NO REGISTO
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;
using ToniEmprega.Data;
using ToniEmprega.Models;

namespace ToniEmprega.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _context.Utilizadores
                .Include(u => u.TipoUtilizador)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Palavra_Passe))
            {
                ViewBag.Error = "Email ou palavra-passe incorretos.";
                return View();
            }

            // Definir session
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.Nome);
            HttpContext.Session.SetString("UserType", user.TipoUtilizador.Designacao);

            // ✅ VERIFICAR ESTADO DA CONTA - se não estiver aprovado, mandar para validação
            if (user.Id_Estado_Validacao_Utilizador == 1) // Pendente
            {
                TempData["Warning"] = "Precisa de completar a validação de identidade.";
                return RedirectToAction("Index", "Validacao");
            }

            if (user.Id_Estado_Validacao_Utilizador == 3) // Rejeitado
            {
                TempData["Error"] = "A sua validação foi rejeitada. Submeta novo documento.";
                return RedirectToAction("Index", "Validacao");
            }

            // ✅ APROVADO - Login normal
            _context.Notificacoes.Add(new Notificacao
            {
                Id_Utilizador = user.Id,
                Titulo = "Novo login",
                Mensagem = $"Login realizado em {DateTime.Now:dd/MM/yyyy HH:mm}",
                Tipo = "info"
            });
            await _context.SaveChangesAsync();

            return user.TipoUtilizador.Designacao switch
            {
                "Aluno" => RedirectToAction("Dashboard", "Aluno"),
                "Professor" => RedirectToAction("Dashboard", "Professor"),
                "Empresa" => RedirectToAction("Dashboard", "Empresa"),
                "Administrador" => RedirectToAction("Dashboard", "Admin"),
                _ => RedirectToAction("Index", "Home")
            };
        }

        public async Task<IActionResult> Register()
        {
            ViewBag.TiposUtilizador = await _context.TipoUtilizadores
                .Where(t => t.Designacao != "Administrador")
                .ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Utilizador utilizador, string confirmPassword, int tipoUtilizadorId)
        {
            // ✅ 2.3 - Validação de Password Complexa
            if (!IsPasswordComplex(utilizador.Palavra_Passe))
            {
                ModelState.AddModelError("", "A palavra-passe deve ter pelo menos 8 caracteres, incluindo maiúsculas, minúsculas, números e caracteres especiais.");
                ViewBag.TiposUtilizador = await _context.TipoUtilizadores
                    .Where(t => t.Designacao != "Administrador")
                    .ToListAsync();
                return View(utilizador);
            }

            if (utilizador.Palavra_Passe != confirmPassword)
            {
                ModelState.AddModelError("", "As palavras-passe não coincidem.");
                ViewBag.TiposUtilizador = await _context.TipoUtilizadores
                    .Where(t => t.Designacao != "Administrador")
                    .ToListAsync();
                return View(utilizador);
            }

            // ✅ 2.1 - Validação de Email (formato e unicidade)
            if (!IsValidEmail(utilizador.Email))
            {
                ModelState.AddModelError("", "Email inválido. Use um formato válido (ex: nome@dominio.pt)");
                ViewBag.TiposUtilizador = await _context.TipoUtilizadores
                    .Where(t => t.Designacao != "Administrador")
                    .ToListAsync();
                return View(utilizador);
            }

            if (await _context.Utilizadores.AnyAsync(u => u.Email == utilizador.Email))
            {
                ModelState.AddModelError("", "Email já registado.");
                ViewBag.TiposUtilizador = await _context.TipoUtilizadores
                    .Where(t => t.Designacao != "Administrador")
                    .ToListAsync();
                return View(utilizador);
            }

            // ✅ 2.2 - Validação de Idade
            if (!utilizador.Data_Nascimento.HasValue)
            {
                ModelState.AddModelError("", "Data de nascimento é obrigatória.");
                ViewBag.TiposUtilizador = await _context.TipoUtilizadores
                    .Where(t => t.Designacao != "Administrador")
                    .ToListAsync();
                return View(utilizador);
            }

            var idade = CalcularIdade(utilizador.Data_Nascimento.Value);
            var tipo = await _context.TipoUtilizadores.FindAsync(tipoUtilizadorId);

            // ✅ 2.2.1 - Aluno >= 17 anos
            if (tipo?.Designacao == "Aluno" && idade < 17)
            {
                ModelState.AddModelError("", "Para se registar como Aluno, deve ter pelo menos 17 anos.");
                ViewBag.TiposUtilizador = await _context.TipoUtilizadores
                    .Where(t => t.Designacao != "Administrador")
                    .ToListAsync();
                return View(utilizador);
            }

            // ✅ 2.2.2 - Utilizador Normal >= 18 anos
            if (tipo?.Designacao == "Utilizador Normal" && idade < 18)
            {
                ModelState.AddModelError("", "Para se registar, deve ter pelo menos 18 anos.");
                ViewBag.TiposUtilizador = await _context.TipoUtilizadores
                    .Where(t => t.Designacao != "Administrador")
                    .ToListAsync();
                return View(utilizador);
            }

            // Resto do código de registo mantém-se...
            utilizador.Id_Tipo_Utilizador = tipoUtilizadorId;
            utilizador.Id_Estado_Validacao_Utilizador = 1; // Pendente
            utilizador.Palavra_Passe = BCrypt.Net.BCrypt.HashPassword(utilizador.Palavra_Passe);
            utilizador.Data_Registro = DateTime.Now;

            _context.Utilizadores.Add(utilizador);
            await _context.SaveChangesAsync();

            // Criar perfil específico...
            switch (tipo?.Designacao)
            {
                case "Aluno":
                    _context.Alunos.Add(new Aluno
                    {
                        Id_Utilizador = utilizador.Id,
                        Curso = "",
                        Ano_Letivo = "",
                        Numero_Aluno = ""
                    });
                    break;
                case "Professor":
                    _context.Professores.Add(new Professor
                    {
                        Id_Utilizador = utilizador.Id,
                        Departamento = "",
                        Numero_Professor = ""
                    });
                    break;
                case "Empresa":
                    _context.Empresas.Add(new Empresa
                    {
                        Id_Utilizador = utilizador.Id,
                        Nome_Empresa = utilizador.Nome,
                        Nif = "",
                        Morada = "",
                        Site_Empresa = "",
                        Telefone = ""
                    });
                    break;
                case "Utilizador Normal":
                    _context.UtilizadoresNormais.Add(new UtilizadorNormal
                    {
                        Id_Utilizador = utilizador.Id,
                        Documentacao_Identificacao = ""
                    });
                    break;
            }
            await _context.SaveChangesAsync();

            // Notificar admins...
            var admins = await _context.Admins.Include(a => a.Utilizador).ToListAsync();
            foreach (var admin in admins)
            {
                _context.Notificacoes.Add(new Notificacao
                {
                    Id_Utilizador = admin.Id_Utilizador,
                    Titulo = "Novo registo pendente",
                    Mensagem = $"{utilizador.Nome} ({tipo?.Designacao}) registou-se e aguarda validação de documento.",
                    Tipo = "warning",
                    Link = "/Admin/Validacoes"
                });
            }
            await _context.SaveChangesAsync();

            HttpContext.Session.SetInt32("UserId", utilizador.Id);
            HttpContext.Session.SetString("UserName", utilizador.Nome);
            HttpContext.Session.SetString("UserType", tipo?.Designacao ?? "Utilizador");

            TempData["Success"] = "Registo efetuado! Agora submeta o seu documento de identificação.";
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
            var user = await _context.Utilizadores.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                // Não revelar se email existe ou não (segurança)
                TempData["Success"] = "Se o email existir no sistema, receberá um código de recuperação.";
                return RedirectToAction("RecuperarPassword");
            }

            // Gerar código de 6 dígitos
            var codigo = new Random().Next(100000, 999999).ToString();

            // Guardar código na sessão (temporário)
            HttpContext.Session.SetString("ResetCode", codigo);
            HttpContext.Session.SetString("ResetEmail", email);
            HttpContext.Session.SetString("ResetTime", DateTime.Now.ToString());

            // Enviar email (simulação - em produção usar serviço de email real)
            // TODO: Configurar SMTP no appsettings.json
            try
            {
                await EnviarEmailRecuperacao(email, codigo, user.Nome);
                TempData["Success"] = "Código de recuperação enviado para o seu email.";
                return RedirectToAction("VerificarCodigo");
            }
            catch
            {
                // Em desenvolvimento, mostrar código na tela
                TempData["Warning"] = $"Modo desenvolvimento: O seu código é {codigo}";
                return RedirectToAction("VerificarCodigo");
            }
        }

        public IActionResult VerificarCodigo()
        {
            if (HttpContext.Session.GetString("ResetCode") == null)
                return RedirectToAction("RecuperarPassword");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult VerificarCodigo(string codigo)
        {
            var storedCode = HttpContext.Session.GetString("ResetCode");
            var resetTime = DateTime.Parse(HttpContext.Session.GetString("ResetTime") ?? DateTime.MinValue.ToString());

            // Código expira em 15 minutos
            if (DateTime.Now > resetTime.AddMinutes(15))
            {
                TempData["Error"] = "Código expirado. Solicite novo código.";
                HttpContext.Session.Remove("ResetCode");
                return RedirectToAction("RecuperarPassword");
            }

            if (codigo != storedCode)
            {
                TempData["Error"] = "Código incorreto.";
                return View();
            }

            return RedirectToAction("NovaPassword");
        }

        // ✅ VIEW NOVA PASSWORD
        public IActionResult NovaPassword()
        {
            if (HttpContext.Session.GetString("ResetCode") == null)
                return RedirectToAction("RecuperarPassword");

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
                return RedirectToAction("RecuperarPassword");
            }

            user.Palavra_Passe = BCrypt.Net.BCrypt.HashPassword(novaPassword);
            await _context.SaveChangesAsync();

            // Limpar sessão
            HttpContext.Session.Remove("ResetCode");
            HttpContext.Session.Remove("ResetEmail");
            HttpContext.Session.Remove("ResetTime");

            TempData["Success"] = "Password alterada com sucesso! Faça login com a nova password.";
            return RedirectToAction("Login");
        }

        private async Task EnviarEmailRecuperacao(string email, string codigo, string nome)
        {
            // Configuração SMTP - deve ser colocada no appsettings.json
            var smtpServer = _configuration["Email:SmtpServer"] ?? "smtp.gmail.com";
            var smtpPort = int.Parse(_configuration["Email:SmtpPort"] ?? "587");
            var smtpUser = _configuration["Email:SmtpUser"] ?? "";
            var smtpPass = _configuration["Email:SmtpPass"] ?? "";
            var fromEmail = _configuration["Email:FromEmail"] ?? "noreply@toniemprega.pt";

            using var client = new SmtpClient(smtpServer, smtpPort)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(smtpUser, smtpPass)
            };

            var message = new MailMessage
            {
                From = new MailAddress(fromEmail, "ToniEmprega"),
                Subject = "Recuperação de Password",
                Body = $@"
            <h2>Recuperação de Password - ToniEmprega</h2>
            <p>Olá {nome},</p>
            <p>Recebemos um pedido para recuperar a password da sua conta.</p>
            <p>O seu código de verificação é:</p>
            <h1 style='color: #1E90FF; font-size: 32px; letter-spacing: 5px;'>{codigo}</h1>
            <p>Este código é válido por 15 minutos.</p>
            <p>Se não solicitou esta recuperação, ignore este email.</p>
            <br>
            <p>Atenciosamente,<br>Equipa ToniEmprega</p>
        ",
                IsBodyHtml = true
            };

            message.To.Add(email);
            await client.SendMailAsync(message);
        }


        private bool IsPasswordComplex(string password)
        {
            if (string.IsNullOrEmpty(password) || password.Length < 8)
                return false;

            bool hasUpper = password.Any(char.IsUpper);
            bool hasLower = password.Any(char.IsLower);
            bool hasDigit = password.Any(char.IsDigit);
            bool hasSpecial = password.Any(c => !char.IsLetterOrDigit(c));

            return hasUpper && hasLower && hasDigit && hasSpecial;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email && email.Contains(".");
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


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Perfil()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue) return RedirectToAction("Login");

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
            if (!userId.HasValue) return RedirectToAction("Login");

            var user = await _context.Utilizadores.FindAsync(userId.Value);
            if (user == null) return NotFound();

            user.Nome = nome;
            user.Data_Nascimento = dataNascimento;
            await _context.SaveChangesAsync();

            HttpContext.Session.SetString("UserName", user.Nome);
            TempData["Success"] = "Perfil atualizado com sucesso!";
            return RedirectToAction("Perfil");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AlterarPassword(string passwordAtual, string novaPassword, string confirmarPassword)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue) return RedirectToAction("Login");

            var user = await _context.Utilizadores.FindAsync(userId.Value);
            if (user == null) return NotFound();

            if (!BCrypt.Net.BCrypt.Verify(passwordAtual, user.Palavra_Passe))
            {
                TempData["Error"] = "Password atual incorreta.";
                return RedirectToAction("Perfil");
            }

            if (novaPassword != confirmarPassword)
            {
                TempData["Error"] = "As novas passwords não coincidem.";
                return RedirectToAction("Perfil");
            }

            user.Palavra_Passe = BCrypt.Net.BCrypt.HashPassword(novaPassword);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Password alterada com sucesso!";
            return RedirectToAction("Perfil");
        }
    }
}