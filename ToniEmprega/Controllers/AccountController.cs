// Controllers/AccountController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
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
        public async Task<IActionResult> Login(string email, string password)
        {
            var hashedPassword = HashPassword(password);
            var user = await _context.Utilizadores
                .Include(u => u.TipoUtilizador)
                .FirstOrDefaultAsync(u => u.Email == email && u.PalavraPasse == hashedPassword);

            if (user == null)
            {
                ViewBag.Error = "Email ou palavra-passe incorretos.";
                return View();
            }

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.Nome);
            HttpContext.Session.SetString("UserType", user.TipoUtilizador.Designacao);

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
        public async Task<IActionResult> Register(Utilizador utilizador, string confirmPassword, int tipoUtilizadorId)
        {
            if (utilizador.PalavraPasse != confirmPassword)
            {
                ModelState.AddModelError("", "As palavras-passe não coincidem.");
                return View(utilizador);
            }

            if (await _context.Utilizadores.AnyAsync(u => u.Email == utilizador.Email))
            {
                ModelState.AddModelError("", "Email já registado.");
                return View(utilizador);
            }

            utilizador.Id_Tipo_Utilizador = tipoUtilizadorId;
            utilizador.Id_Estado_Validacao_Utilizador = 1;
            utilizador.PalavraPasse = HashPassword(utilizador.PalavraPasse);

            _context.Utilizadores.Add(utilizador);
            await _context.SaveChangesAsync();

            var tipo = await _context.TipoUtilizadores.FindAsync(tipoUtilizadorId);
            switch (tipo?.Designacao)
            {
                case "Aluno":
                    _context.Alunos.Add(new Aluno { Id_Utilizador = utilizador.Id });
                    break;
                case "Professor":
                    _context.Professores.Add(new Professor { Id_Utilizador = utilizador.Id });
                    break;
                case "Empresa":
                    _context.Empresas.Add(new Empresa { Id_Utilizador = utilizador.Id, NomeEmpresa = utilizador.Nome });
                    break;
            }
            await _context.SaveChangesAsync();

            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            return Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }
    }
}