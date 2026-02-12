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
            return View(await ofertas.OrderByDescending(o => o.Data_Publicacao).ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var oferta = await _context.Ofertas
                .Include(o => o.Empresa)
                .Include(o => o.TipoOferta)
                .Include(o => o.EstadoOferta)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (oferta == null) return NotFound();

            // Verificar se aluno já candidatou
            var userId = HttpContext.Session.GetInt32("UserId");
            var userType = HttpContext.Session.GetString("UserType");

            if (userId.HasValue && userType == "Aluno")
            {
                var aluno = await _context.Alunos.FirstOrDefaultAsync(a => a.Id_Utilizador == userId);
                if (aluno != null)
                {
                    ViewBag.JaCandidatou = await _context.Candidaturas
                        .AnyAsync(c => c.Id_Oferta == id && c.Id_Aluno == aluno.Id);
                }
            }

            return View(oferta);
        }


    }
}