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

        // ========== PÁGINA PRINCIPAL ==========
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue) return RedirectToAction("Login", "Account");

            var notificacoes = await _context.Notificacoes
                .Where(n => n.Id_Utilizador == userId.Value)
                .OrderByDescending(n => n.Data_Criacao)
                .ToListAsync();

            // Marcar todas como lidas ao visualizar a página
            var naoLidas = notificacoes.Where(n => !n.Lida).ToList();
            foreach (var n in naoLidas)
            {
                n.Lida = true;
            }
            await _context.SaveChangesAsync();

            return View(notificacoes);
        }

        // ========== API: LISTAR RECENTES (para dropdown) ==========
        [HttpGet]
        public async Task<IActionResult> ListaRecentes(int limite = 10)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue) return Json(new List<object>());

            var notificacoes = await _context.Notificacoes
                .Where(n => n.Id_Utilizador == userId.Value)
                .OrderByDescending(n => n.Data_Criacao)
                .Take(limite)
                .Select(n => new
                {
                    n.Id,
                    n.Titulo,
                    n.Mensagem,
                    n.Tipo,
                    n.Lida,
                    n.Link,
                    Data_Criacao = n.Data_Criacao.ToString("o")
                })
                .ToListAsync();

            return Json(notificacoes);
        }

        // ========== API: MARCAR COMO LIDA ==========
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarcarComoLida(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue) return Unauthorized();

            var notificacao = await _context.Notificacoes
                .FirstOrDefaultAsync(n => n.Id == id && n.Id_Utilizador == userId.Value);

            if (notificacao != null && !notificacao.Lida)
            {
                notificacao.Lida = true;
                await _context.SaveChangesAsync();
            }

            return Ok();
        }

        // ========== API: MARCAR TODAS COMO LIDAS ==========
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarcarTodasComoLidas()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue) return Unauthorized();

            var naoLidas = await _context.Notificacoes
                .Where(n => n.Id_Utilizador == userId.Value && !n.Lida)
                .ToListAsync();

            foreach (var n in naoLidas)
            {
                n.Lida = true;
            }

            await _context.SaveChangesAsync();
            return Ok(new { count = naoLidas.Count });
        }

        // ========== API: CONTADOR ==========
        public async Task<IActionResult> Contador()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue) return Json(0);

            var count = await _context.Notificacoes
                .CountAsync(n => n.Id_Utilizador == userId.Value && !n.Lida);

            return Json(count);
        }

        // ========== API: VERIFICAR NOVAS ==========
        [HttpGet]
        public async Task<IActionResult> VerificarNovas(DateTime? ultimaVerificacao)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue) return Json(new { count = 0, notificacoes = new List<object>() });

            var query = _context.Notificacoes
                .Where(n => n.Id_Utilizador == userId.Value && !n.Lida);

            if (ultimaVerificacao.HasValue)
            {
                query = query.Where(n => n.Data_Criacao > ultimaVerificacao.Value);
            }

            var notificacoes = await query
                .OrderByDescending(n => n.Data_Criacao)
                .Select(n => new
                {
                    n.Id,
                    n.Titulo,
                    n.Mensagem,
                    n.Tipo,
                    n.Link,
                    Data_Criacao = n.Data_Criacao.ToString("o")
                })
                .ToListAsync();

            return Json(new
            {
                count = notificacoes.Count,
                notificacoes
            });
        }
    }
}