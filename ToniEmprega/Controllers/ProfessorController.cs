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
                .CountAsync(c => c.Id_Estado_Candidatura == 1 || c.Id_Estado_Candidatura == 2);

            var minhasAvaliacoes = await _context.AvaliacoesProfessores
                .CountAsync(a => a.Id_Professor == professor.Id);

            ViewBag.AvaliacoesPendentes = avaliacoesPendentes;
            ViewBag.MinhasAvaliacoes = minhasAvaliacoes;

            // Notificações
            ViewBag.Notificacoes = await _context.Notificacoes
                .Where(n => n.Id_Utilizador == professor.Id_Utilizador && !n.Lida)
                .OrderByDescending(n => n.Data_Criacao)
                .Take(5)
                .ToListAsync();

            return View();
        }

        public async Task<IActionResult> Candidaturas()
        {
            var professor = await GetCurrentProfessor();
            if (professor == null) return RedirectToAction("Login", "Account");

            // Obter IDs das candidaturas já avaliadas por este professor
            var candidaturasAvaliadasIds = await _context.AvaliacoesProfessores
                .Where(a => a.Id_Professor == professor.Id)
                .Select(a => a.Id_Candidatura)
                .ToListAsync();

            // Mostrar candidaturas pendentes (estado 1 ou 2) E candidaturas já avaliadas por este professor
            var candidaturas = await _context.Candidaturas
                .Include(c => c.Aluno)
                .ThenInclude(a => a.Utilizador)
                .Include(c => c.Oferta)
                .ThenInclude(o => o.Empresa)
                .Include(c => c.EstadoCandidatura)
                .Include(c => c.Ficheiros)
                .Include(c => c.Avaliacoes)
                    .ThenInclude(a => a.DecisaoAvaliacao)
                .Where(c => c.Id_Estado_Candidatura == 1 || c.Id_Estado_Candidatura == 2
                    || candidaturasAvaliadasIds.Contains(c.Id))
                .OrderByDescending(c => c.Data_Candidatura)
                .ToListAsync();

            ViewBag.ProfessorId = professor.Id;
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

            // Verificar se já foi avaliada por este professor
            var jaAvaliou = await _context.AvaliacoesProfessores
                .AnyAsync(a => a.Id_Candidatura == id && a.Id_Professor == professor.Id);

            if (jaAvaliou)
            {
                TempData["Error"] = "Já avaliou esta candidatura.";
                return RedirectToAction("Candidaturas");
            }

            ViewBag.Decisoes = await _context.DecisaoAvaliacoes.ToListAsync();
            return View(candidatura);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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

            var candidatura = await _context.Candidaturas
                .Include(c => c.Aluno)
                .ThenInclude(a => a.Utilizador)
                .Include(c => c.Oferta)
                .FirstAsync(c => c.Id == candidaturaId);

            // Atualizar estado da candidatura
            candidatura.Id_Estado_Candidatura = decisaoId switch
            {
                1 => 3, // Aprovado -> Candidatura Aprovada
                2 => 4, // Rejeitado -> Candidatura Rejeitada
                _ => 2  // Necessita Revisão -> Em Análise
            };

            await _context.SaveChangesAsync();

            // Notificar aluno
            _context.Notificacoes.Add(new Notificacao
            {
                Id_Utilizador = candidatura.Aluno.Id_Utilizador,
                Titulo = "Candidatura avaliada!",
                Mensagem = $"A sua candidatura para '{candidatura.Oferta?.Titulo}' foi avaliada.",
                Tipo = decisaoId == 1 ? "success" : "warning",
                Link = "/Aluno/MinhasCandidaturas"
            });
            await _context.SaveChangesAsync();

            TempData["Success"] = "Avaliação registada com sucesso!";
            return RedirectToAction("Candidaturas");
        }

        public async Task<IActionResult> MinhasAvaliacoes()
        {
            var professor = await GetCurrentProfessor();
            if (professor == null) return RedirectToAction("Login", "Account");

            var avaliacoes = await _context.AvaliacoesProfessores
                .Include(a => a.Candidatura)
                .ThenInclude(c => c.Aluno)
                .ThenInclude(a => a.Utilizador)
                .Include(a => a.Candidatura)
                .ThenInclude(c => c.Oferta)
                .Include(a => a.DecisaoAvaliacao)
                .Where(a => a.Id_Professor == professor.Id)
                .OrderByDescending(a => a.Data_Avaliacao)
                .ToListAsync();

            return View(avaliacoes);
        }


        // EDITAR AVALIAÇÃO - GET
        public async Task<IActionResult> EditarAvaliacao(int? id)
        {
            if (id == null) return NotFound();

            var professor = await GetCurrentProfessor();
            if (professor == null) return RedirectToAction("Login", "Account");

            var avaliacao = await _context.AvaliacoesProfessores
                .Include(a => a.Candidatura)
                .ThenInclude(c => c.Aluno)
                .ThenInclude(a => a.Utilizador)
                .Include(a => a.Candidatura)
                .ThenInclude(c => c.Oferta)
                .Include(a => a.Candidatura)
                .ThenInclude(c => c.Ficheiros)
                .Include(a => a.DecisaoAvaliacao)
                .FirstOrDefaultAsync(a => a.Id == id && a.Id_Professor == professor.Id);

            if (avaliacao == null) return NotFound();

            ViewBag.Decisoes = await _context.DecisaoAvaliacoes.ToListAsync();
            return View(avaliacao);
        }

        // EDITAR AVALIAÇÃO - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarAvaliacao(int id, int decisaoId, string comentarios)
        {
            var professor = await GetCurrentProfessor();
            if (professor == null) return RedirectToAction("Login", "Account");

            var avaliacao = await _context.AvaliacoesProfessores
                .Include(a => a.Candidatura)
                .ThenInclude(c => c.Aluno)
                .ThenInclude(a => a.Utilizador)
                .Include(a => a.Candidatura)
                .ThenInclude(c => c.Oferta)
                .FirstOrDefaultAsync(a => a.Id == id && a.Id_Professor == professor.Id);

            if (avaliacao == null) return NotFound();

            // Atualizar avaliação
            avaliacao.Id_Decisao_Avaliacao = decisaoId;
            avaliacao.Comentarios = comentarios;
            avaliacao.Data_Avaliacao = DateTime.Now;

            // Atualizar estado da candidatura conforme nova decisão
            var candidatura = avaliacao.Candidatura;
            candidatura.Id_Estado_Candidatura = decisaoId switch
            {
                1 => 3, // Aprovado → Candidatura Aprovada
                2 => 4, // Rejeitado → Candidatura Rejeitada
                3 => 2, // Necessita Revisão → Em Análise
                _ => 2
            };

            await _context.SaveChangesAsync();

            // Notificar aluno sobre alteração
            _context.Notificacoes.Add(new Notificacao
            {
                Id_Utilizador = candidatura.Aluno.Id_Utilizador,
                Titulo = "Avaliação atualizada!",
                Mensagem = $"A sua candidatura para '{candidatura.Oferta?.Titulo}' teve a avaliação atualizada.",
                Tipo = decisaoId == 1 ? "success" : "warning",
                Link = "/Aluno/MinhasCandidaturas"
            });
            await _context.SaveChangesAsync();

            TempData["Success"] = "Avaliação atualizada com sucesso!";
            return RedirectToAction("MinhasAvaliacoes");
        }

        // PERFIL DO PROFESSOR
        public async Task<IActionResult> Perfil()
        {
            var professor = await GetCurrentProfessor();
            if (professor == null) return RedirectToAction("Login", "Account");

            return View(professor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AtualizarPerfil(string departamento, string numeroProfessor)
        {
            var professor = await GetCurrentProfessor();
            if (professor == null) return RedirectToAction("Login", "Account");

            professor.Departamento = departamento;
            professor.Numero_Professor = numeroProfessor;
            await _context.SaveChangesAsync();

            TempData["Success"] = "Perfil atualizado com sucesso!";
            return RedirectToAction("Perfil");
        }
    }
}