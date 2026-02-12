// Controllers/EmpresaController.cs (ATUALIZADO COMPLETO)
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
                .Include(o => o.EstadoOferta)
                .Where(o => o.Id_Empresa == empresa.Id)
                .OrderByDescending(o => o.Data_Publicacao)
                .Take(5)
                .ToListAsync();

            ViewBag.TotalOfertas = await _context.Ofertas.CountAsync(o => o.Id_Empresa == empresa.Id);
            ViewBag.OfertasAtivas = await _context.Ofertas
                .CountAsync(o => o.Id_Empresa == empresa.Id && o.Id_Estado_Oferta == 1);
            ViewBag.TotalCandidaturas = await _context.Candidaturas
                .CountAsync(c => c.Oferta.Id_Empresa == empresa.Id);

            return View(ofertas);
        }

        public async Task<IActionResult> MinhasOfertas()
        {
            var empresa = await GetCurrentEmpresa();
            if (empresa == null) return RedirectToAction("Login", "Account");

            var ofertas = await _context.Ofertas
                .Include(o => o.TipoOferta)
                .Include(o => o.EstadoOferta)
                .Include(o => o.Candidaturas)
                .Where(o => o.Id_Empresa == empresa.Id)
                .OrderByDescending(o => o.Data_Publicacao)
                .ToListAsync();

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
            oferta.Id_Estado_Oferta = 1; // Ativa
            oferta.Data_Publicacao = DateTime.Now;

            _context.Ofertas.Add(oferta);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Oferta criada com sucesso!";
            return RedirectToAction("MinhasOfertas");
        }

        public async Task<IActionResult> Candidatos(int? id)
        {
            if (id == null) return NotFound();

            var empresa = await GetCurrentEmpresa();
            if (empresa == null) return RedirectToAction("Login", "Account");

            var oferta = await _context.Ofertas
                .FirstOrDefaultAsync(o => o.Id == id && o.Id_Empresa == empresa.Id);

            if (oferta == null) return NotFound();

            var candidaturas = await _context.Candidaturas
                .Include(c => c.Aluno)
                .ThenInclude(a => a.Utilizador)
                .Include(c => c.EstadoCandidatura)
                .Include(c => c.Ficheiros)
                .Include(c => c.Avaliacoes)
                .ThenInclude(a => a.DecisaoAvaliacao)
                .Where(c => c.Id_Oferta == id)
                .ToListAsync();

            ViewBag.Oferta = oferta;
            return View(candidaturas);
        }
    }
}