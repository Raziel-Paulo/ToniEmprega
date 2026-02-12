// Controllers/AlunoController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToniEmprega.Data;
using ToniEmprega.Models;

namespace ToniEmprega.Controllers
{
    public class AlunoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AlunoController(ApplicationDbContext context)
        {
            _context = context;
        }

        private async Task<Aluno?> GetCurrentAluno()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue) return null;
            return await _context.Alunos
                .Include(a => a.Utilizador)
                .FirstOrDefaultAsync(a => a.Id_Utilizador == userId.Value);
        }

        public async Task<IActionResult> Dashboard()
        {
            var aluno = await GetCurrentAluno();
            if (aluno == null) return RedirectToAction("Login", "Account");

            var candidaturas = await _context.Candidaturas
                .Include(c => c.Oferta)
                .ThenInclude(o => o.Empresa)
                .Where(c => c.Id_Aluno == aluno.Id)
                .OrderByDescending(c => c.DataCandidatura)
                .Take(5)
                .ToListAsync();

            ViewBag.TotalCandidaturas = await _context.Candidaturas.CountAsync(c => c.Id_Aluno == aluno.Id);
            ViewBag.CandidaturasPendentes = await _context.Candidaturas
                .CountAsync(c => c.Id_Aluno == aluno.Id && c.Id_Estado_Candidatura == 1);

            return View(candidaturas);
        }
    }
}