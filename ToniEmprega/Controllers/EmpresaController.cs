// Controllers/EmpresaController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToniEmprega.Data;
using ToniEmprega.Models;

namespace ToniEmprega.Controllers
{
    public class EmpresaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmpresaController(ApplicationDbContext context)
        {
            _context = context;
        }

        private async Task<Empresa?> GetCurrentEmpresa()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue) return null;
            return await _context.Empresas
                .Include(e => e.Utilizador)
                .FirstOrDefaultAsync(e => e.Id_Utilizador == userId.Value);
        }

        public async Task<IActionResult> Dashboard()
        {
            var empresa = await GetCurrentEmpresa();
            if (empresa == null) return RedirectToAction("Login", "Account");

            var ofertas = await _context.Ofertas
                .Include(o => o.TipoOferta)
                .Where(o => o.Id_Empresa == empresa.Id)
                .OrderByDescending(o => o.DataPublicacao)
                .Take(5)
                .ToListAsync();

            ViewBag.TotalOfertas = await _context.Ofertas.CountAsync(o => o.Id_Empresa == empresa.Id);
            ViewBag.OfertasAtivas = await _context.Ofertas
                .CountAsync(o => o.Id_Empresa == empresa.Id && o.Id_Estado_Oferta == 1);

            return View(ofertas);
        }

        public async Task<IActionResult> CriarOferta()
        {
            ViewBag.TiposOferta = await _context.TipoOfertas.ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CriarOferta(Oferta oferta)
        {
            var empresa = await GetCurrentEmpresa();
            if (empresa == null) return RedirectToAction("Login", "Account");

            oferta.Id_Empresa = empresa.Id;
            oferta.Id_Estado_Oferta = 1;
            oferta.DataPublicacao = DateTime.Now;

            _context.Ofertas.Add(oferta);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Oferta criada com sucesso!";
            return RedirectToAction("Dashboard");
        }
    }
}