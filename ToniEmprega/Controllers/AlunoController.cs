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
            ViewBag.CandidaturasAprovadas = await _context.Candidaturas
                .CountAsync(c => c.Id_Aluno == aluno.Id && c.Id_Estado_Candidatura == 3);

            ViewBag.Notificacoes = await _context.Notificacoes
                .Where(n => n.Id_Utilizador == aluno.Id_Utilizador && !n.Lida)
                .OrderByDescending(n => n.Data_Criacao)
                .Take(5)
                .ToListAsync();

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelarCandidatura(int id)
        {
            var aluno = await GetCurrentAluno();
            var candidatura = await _context.Candidaturas
                .FirstOrDefaultAsync(c => c.Id == id && c.Id_Aluno == aluno!.Id);

            if (candidatura == null) return NotFound();

            if (candidatura.Id_Estado_Candidatura == 1 || candidatura.Id_Estado_Candidatura == 2)
            {
                candidatura.Id_Estado_Candidatura = 5; // Cancelada
                await _context.SaveChangesAsync();

                var oferta = await _context.Ofertas
                    .Include(o => o.Empresa)
                    .FirstAsync(o => o.Id == candidatura.Id_Oferta);

                _context.Notificacoes.Add(new Notificacao
                {
                    Id_Utilizador = oferta.Empresa.Id_Utilizador,
                    Titulo = "Candidatura cancelada",
                    Mensagem = $"Um aluno cancelou a candidatura à oferta '{oferta.Titulo}'",
                    Tipo = "warning"
                });
                await _context.SaveChangesAsync();

                TempData["Success"] = "Candidatura cancelada com sucesso.";
            }
            else
            {
                TempData["Error"] = "Não é possível cancelar esta candidatura (já foi processada).";
            }

            return RedirectToAction("MinhasCandidaturas");
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

            if (aluno.Utilizador.Id_Estado_Validacao_Utilizador != 2)
            {
                TempData["Error"] = "Precisa de ter a identidade validada para se candidatar.";
                return RedirectToAction("Index", "Validacao");
            }

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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Candidatar(int ofertaId, string mensagem, List<IFormFile> ficheiros)
        {
            var aluno = await GetCurrentAluno();
            if (aluno == null) return RedirectToAction("Login", "Account");

            // VALIDAÇÃO DE FICHEIROS - CORRIGIDO
            if (ficheiros != null && ficheiros.Count > 0)
            {
                var allowedExtensions = new[] { ".pdf", ".doc", ".docx" };
                var allowedTypes = new[] { "application/pdf", "application/msword",
                    "application/vnd.openxmlformats-officedocument.wordprocessingml.document" };
                const long maxSize = 5 * 1024 * 1024; // 5MB

                foreach (var ficheiro in ficheiros)
                {
                    // CORREÇÃO: Declarar ext fora do if para usar depois
                    var ext = Path.GetExtension(ficheiro.FileName).ToLower();

                    if (!allowedExtensions.Contains(ext) || !allowedTypes.Contains(ficheiro.ContentType))
                    {
                        TempData["Error"] = "Tipo de ficheiro não permitido. Use apenas PDF, DOC ou DOCX.";
                        return RedirectToAction("Candidatar", new { id = ofertaId });
                    }
                    if (ficheiro.Length > maxSize)
                    {
                        TempData["Error"] = "Ficheiro demasiado grande (máximo 5MB).";
                        return RedirectToAction("Candidatar", new { id = ofertaId });
                    }
                }
            }

            var candidatura = new Candidatura
            {
                Id_Oferta = ofertaId,
                Id_Aluno = aluno.Id,
                Id_Estado_Candidatura = 1,
                Data_Candidatura = DateTime.Now,
                Mensagem = mensagem
            };

            _context.Candidaturas.Add(candidatura);
            await _context.SaveChangesAsync();

            // Processar ficheiros
            if (ficheiros != null && ficheiros.Count > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "candidaturas");
                Directory.CreateDirectory(uploadsFolder);

                foreach (var ficheiro in ficheiros)
                {
                    if (ficheiro.Length > 0)
                    {
                        var ext = Path.GetExtension(ficheiro.FileName).ToLower(); // CORREÇÃO: declarar novamente aqui
                        var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(ficheiro.FileName)}";
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await ficheiro.CopyToAsync(stream);
                        }

                        _context.CandidaturaFicheiros.Add(new CandidaturaFicheiro
                        {
                            Id_Candidatura = candidatura.Id,
                            Tipo_Ficheiro = ext == ".pdf" ? "CV" : "Anexo",
                            Nome_Ficheiro = ficheiro.FileName,
                            Caminho_Ficheiro = $"/uploads/candidaturas/{uniqueFileName}",
                            Data_Upload = DateTime.Now
                        });
                    }
                }
                await _context.SaveChangesAsync();
            }

            var oferta = await _context.Ofertas
                .Include(o => o.Empresa)
                .FirstAsync(o => o.Id == ofertaId);

            _context.Notificacoes.Add(new Notificacao
            {
                Id_Utilizador = oferta.Empresa.Id_Utilizador,
                Titulo = "Nova candidatura!",
                Mensagem = $"Recebeu uma nova candidatura para '{oferta.Titulo}'",
                Tipo = "success",
                Link = $"/Empresa/Candidatos/{ofertaId}"
            });
            await _context.SaveChangesAsync();

            TempData["Success"] = "Candidatura submetida com sucesso!";
            return RedirectToAction("MinhasCandidaturas");
        }

        public async Task<IActionResult> Perfil()
        {
            var aluno = await GetCurrentAluno();
            if (aluno == null) return RedirectToAction("Login", "Account");

            ViewBag.Cursos = new[] { "Informática", "Mecatrónica", "Eletrónica", "Gestão", "Turismo" };
            return View(aluno);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AtualizarPerfil(string curso, string anoLetivo, string numeroAluno)
        {
            var aluno = await GetCurrentAluno();
            if (aluno == null) return RedirectToAction("Login", "Account");

            aluno.Curso = curso;
            aluno.Ano_Letivo = anoLetivo;
            aluno.Numero_Aluno = numeroAluno;
            await _context.SaveChangesAsync();

            TempData["Success"] = "Perfil académico atualizado!";
            return RedirectToAction("Perfil");
        }
    }
}