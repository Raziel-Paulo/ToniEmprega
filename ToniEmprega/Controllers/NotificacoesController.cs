using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToniEmprega.Data;
using ToniEmprega.Models;

namespace ToniEmprega.Controllers
{
    public class NotificacoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NotificacoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue) return RedirectToAction("Login", "Account");

            var notificacoes = await _context.Notificacoes
                .Where(n => n.Id_Utilizador == userId.Value)
                .OrderByDescending(n => n.Data_Criacao)
                .ToListAsync();

            // Marcar todas como lidas ao visualizar
            var naoLidas = notificacoes.Where(n => !n.Lida).ToList();
            foreach (var n in naoLidas)
            {
                n.Lida = true;
            }
            await _context.SaveChangesAsync();

            return View(notificacoes);
        }

        [HttpPost]
        public async Task<IActionResult> MarcarComoLida(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var notificacao = await _context.Notificacoes
                .FirstOrDefaultAsync(n => n.Id == id && n.Id_Utilizador == userId);

            if (notificacao != null)
            {
                notificacao.Lida = true;
                await _context.SaveChangesAsync();
            }

            return Ok();
        }

        public async Task<IActionResult> Contador()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue) return Json(0);

            var count = await _context.Notificacoes
                .CountAsync(n => n.Id_Utilizador == userId.Value && !n.Lida);

            return Json(count);
        }
    }
}