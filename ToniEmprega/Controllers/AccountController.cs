// Controllers/AccountController.cs - COMPLETO COM VALIDAÇÃO NO REGISTO
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            if (utilizador.Palavra_Passe != confirmPassword)
            {
                ModelState.AddModelError("", "As palavras-passe não coincidem.");
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

            utilizador.Id_Tipo_Utilizador = tipoUtilizadorId;
            utilizador.Id_Estado_Validacao_Utilizador = 1; // Pendente
            utilizador.Palavra_Passe = BCrypt.Net.BCrypt.HashPassword(utilizador.Palavra_Passe);
            utilizador.Data_Registro = DateTime.Now;

            _context.Utilizadores.Add(utilizador);
            await _context.SaveChangesAsync();

            // Criar perfil específico
            var tipo = await _context.TipoUtilizadores.FindAsync(tipoUtilizadorId);
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

            // Notificar admins
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

            // ✅ SÓ AGORA definir session - mas redirecionar imediatamente para validação
            HttpContext.Session.SetInt32("UserId", utilizador.Id);
            HttpContext.Session.SetString("UserName", utilizador.Nome);
            HttpContext.Session.SetString("UserType", tipo?.Designacao ?? "Utilizador");

            // ✅ REDIRECIONAR DIRETAMENTE PARA VALIDAÇÃO - não pode ir a lado nenhum antes
            TempData["Success"] = "Registo efetuado! Agora submeta o seu documento de identificação.";
            return RedirectToAction("Index", "Validacao");
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