// Controllers/OfertasController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToniEmprega.Data;
using ToniEmprega.Models;

namespace ToniEmprega.Controllers
{
    public class OfertasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OfertasController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString, int? tipoOfertaId)
        {
            var ofertas = _context.Ofertas
                .Include(o => o.Empresa)
                .Include(o => o.TipoOferta)
                .Where(o => o.Id_Estado_Oferta == 1)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
                ofertas = ofertas.Where(o => o.Titulo.Contains(searchString) || o.Descricao.Contains(searchString));

            if (tipoOfertaId.HasValue)
                ofertas = ofertas.Where(o => o.Id_Tipo_Oferta == tipoOfertaId);

            ViewBag.TiposOferta = await _context.TipoOfertas.ToListAsync();
            return View(await ofertas.OrderByDescending(o => o.DataPublicacao).ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var oferta = await _context.Ofertas
                .Include(o => o.Empresa)
                .Include(o => o.TipoOferta)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (oferta == null) return NotFound();

            return View(oferta);
        }
    }
}