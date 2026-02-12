// Controllers/AlunoController.cs (ATUALIZADO COMPLETO)
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
                .Include(c => c.EstadoCandidatura)
                .Where(c => c.Id_Aluno == aluno.Id)
                .OrderByDescending(c => c.Data_Candidatura)
                .Take(5)
                .ToListAsync();

            ViewBag.TotalCandidaturas = await _context.Candidaturas.CountAsync(c => c.Id_Aluno == aluno.Id);
            ViewBag.CandidaturasPendentes = await _context.Candidaturas
                .CountAsync(c => c.Id_Aluno == aluno.Id && c.Id_Estado_Candidatura == 1);

            return View(candidaturas);
        }

        public async Task<IActionResult> MinhasCandidaturas()
        {
            var aluno = await GetCurrentAluno();
            if (aluno == null) return RedirectToAction("Login", "Account");

            var candidaturas = await _context.Candidaturas
                .Include(c => c.Oferta)
                .ThenInclude(o => o.Empresa)
                .Include(c => c.EstadoCandidatura)
                .Include(c => c.Avaliacoes)
                .ThenInclude(a => a.DecisaoAvaliacao)
                .Where(c => c.Id_Aluno == aluno.Id)
                .OrderByDescending(c => c.Data_Candidatura)
                .ToListAsync();

            return View(candidaturas);
        }

        public async Task<IActionResult> Candidatar(int? id)
        {
            if (id == null) return NotFound();

            var oferta = await _context.Ofertas
                .Include(o => o.Empresa)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (oferta == null) return NotFound();

            var aluno = await GetCurrentAluno();
            if (aluno == null) return RedirectToAction("Login", "Account");

            var jaCandidatou = await _context.Candidaturas
                .AnyAsync(c => c.Id_Oferta == id && c.Id_Aluno == aluno.Id);

            if (jaCandidatou)
            {
                TempData["Error"] = "Já te candidataste a esta oferta.";
                return RedirectToAction("Details", "Ofertas", new { id });
            }

            ViewBag.Oferta = oferta;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Candidatar(int ofertaId, string mensagem, List<IFormFile> ficheiros)
        {
            var aluno = await GetCurrentAluno();
            if (aluno == null) return RedirectToAction("Login", "Account");

            var candidatura = new Candidatura
            {
                Id_Oferta = ofertaId,
                Id_Aluno = aluno.Id,
                Id_Estado_Candidatura = 1, // Pendente
                Data_Candidatura = DateTime.Now,
                Mensagem = mensagem
            };

            _context.Candidaturas.Add(candidatura);
            await _context.SaveChangesAsync();

            // Processar ficheiros
            if (ficheiros != null && ficheiros.Count > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "candidaturas");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                foreach (var ficheiro in ficheiros)
                {
                    if (ficheiro.Length > 0)
                    {
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + ficheiro.FileName;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await ficheiro.CopyToAsync(stream);
                        }

                        _context.CandidaturaFicheiros.Add(new CandidaturaFicheiro
                        {
                            Id_Candidatura = candidatura.Id,
                            Tipo_Ficheiro = Path.GetExtension(ficheiro.FileName).ToLower() == ".pdf" ? "CV" : "Anexo",
                            Nome_Ficheiro = ficheiro.FileName,
                            Caminho_Ficheiro = "/uploads/candidaturas/" + uniqueFileName
                        });
                    }
                }
                await _context.SaveChangesAsync();
            }

            TempData["Success"] = "Candidatura submetida com sucesso!";
            return RedirectToAction("MinhasCandidaturas");
        }
    }
}