// Controllers/ProfessorController.cs (ATUALIZADO COMPLETO)
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToniEmprega.Data;
using ToniEmprega.Models;

namespace ToniEmprega.Controllers
{
    public class ProfessorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProfessorController(ApplicationDbContext context)
        {
            _context = context;
        }

        private async Task<Professor?> GetCurrentProfessor()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue) return null;
            return await _context.Professores
                .Include(p => p.Utilizador)
                .FirstOrDefaultAsync(p => p.Id_Utilizador == userId.Value);
        }

        public async Task<IActionResult> Dashboard()
        {
            var professor = await GetCurrentProfessor();
            if (professor == null) return RedirectToAction("Login", "Account");

            var avaliacoesPendentes = await _context.Candidaturas
                .Include(c => c.Aluno)
                .ThenInclude(a => a.Utilizador)
                .Include(c => c.Oferta)
                .Where(c => c.Id_Estado_Candidatura == 2) // Em Análise
                .CountAsync();

            var minhasAvaliacoes = await _context.AvaliacoesProfessores
                .CountAsync(a => a.Id_Professor == professor.Id);

            ViewBag.AvaliacoesPendentes = avaliacoesPendentes;
            ViewBag.MinhasAvaliacoes = minhasAvaliacoes;

            return View();
        }

        public async Task<IActionResult> Candidaturas()
        {
            var professor = await GetCurrentProfessor();
            if (professor == null) return RedirectToAction("Login", "Account");

            var candidaturas = await _context.Candidaturas
                .Include(c => c.Aluno)
                .ThenInclude(a => a.Utilizador)
                .Include(c => c.Oferta)
                .ThenInclude(o => o.Empresa)
                .Include(c => c.EstadoCandidatura)
                .Include(c => c.Avaliacoes)
                .Where(c => c.Id_Estado_Candidatura == 1 || c.Id_Estado_Candidatura == 2)
                .ToListAsync();

            return View(candidaturas);
        }

        public async Task<IActionResult> Avaliar(int? id)
        {
            if (id == null) return NotFound();

            var professor = await GetCurrentProfessor();
            if (professor == null) return RedirectToAction("Login", "Account");

            var candidatura = await _context.Candidaturas
                .Include(c => c.Aluno)
                .ThenInclude(a => a.Utilizador)
                .Include(c => c.Oferta)
                .ThenInclude(o => o.Empresa)
                .Include(c => c.Ficheiros)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (candidatura == null) return NotFound();

            ViewBag.Decisoes = await _context.DecisaoAvaliacoes.ToListAsync();
            return View(candidatura);
        }

        [HttpPost]
        public async Task<IActionResult> Avaliar(int candidaturaId, int decisaoId, string comentarios)
        {
            var professor = await GetCurrentProfessor();
            if (professor == null) return RedirectToAction("Login", "Account");

            var avaliacao = new AvaliacaoProfessor
            {
                Id_Candidatura = candidaturaId,
                Id_Professor = professor.Id,
                Id_Decisao_Avaliacao = decisaoId,
                Comentarios = comentarios,
                Data_Avaliacao = DateTime.Now
            };

            _context.AvaliacoesProfessores.Add(avaliacao);

            // Atualizar estado da candidatura
            var candidatura = await _context.Candidaturas.FindAsync(candidaturaId);
            if (candidatura != null)
            {
                candidatura.Id_Estado_Candidatura = decisaoId == 1 ? 3 : 4; // Aprovada ou Rejeitada
            }

            await _context.SaveChangesAsync();

            TempData["Success"] = "Avaliação registada com sucesso!";
            return RedirectToAction("Candidaturas");
        }
    }
}