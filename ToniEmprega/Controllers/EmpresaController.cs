using Humanizer.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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

        private static void ValidarOfertaBasica(Oferta oferta, ModelStateDictionary modelState)
        {
            if (string.IsNullOrWhiteSpace(oferta.Titulo) || oferta.Titulo.Length < 5)
                modelState.AddModelError(string.Empty, "O título deve ter pelo menos 5 caracteres.");

            if (string.IsNullOrWhiteSpace(oferta.Descricao) || oferta.Descricao.Length < 20)
                modelState.AddModelError(string.Empty, "A descrição deve ter pelo menos 20 caracteres.");

            if (string.IsNullOrWhiteSpace(oferta.Requisitos) || oferta.Requisitos.Length < 10)
                modelState.AddModelError(string.Empty, "Os requisitos devem ter pelo menos 10 caracteres.");

            if (string.IsNullOrWhiteSpace(oferta.Localizacao) || oferta.Localizacao.Length < 5)
                modelState.AddModelError(string.Empty, "A localização deve ter pelo menos 5 caracteres.");
        }

        public async Task<IActionResult> Dashboard()
        {
            var empresa = await GetCurrentEmpresa();
            if (empresa == null) return RedirectToAction("Login", "Account");

            var ofertas = await _context.Ofertas
                .Include(o => o.TipoOferta)
                .Include(o => o.EstadoOferta)
                .Include(o => o.Candidaturas)
                .ThenInclude(c => c.Aluno)
                .Where(o => o.Id_Empresa == empresa.Id)
                .OrderByDescending(o => o.Data_Publicacao)
                .Take(5)
                .ToListAsync();

            ViewBag.TotalOfertas = await _context.Ofertas
                .CountAsync(o => o.Id_Empresa == empresa.Id);

            ViewBag.OfertasAtivas = await _context.Ofertas
                .CountAsync(o => o.Id_Empresa == empresa.Id && o.Id_Estado_Oferta == 1);

            ViewBag.TotalCandidaturas = await _context.Candidaturas
                .Where(c => c.Oferta.Id_Empresa == empresa.Id)
                .CountAsync();

            ViewBag.CandidaturasPendentes = await _context.Candidaturas
                .Where(c => c.Oferta.Id_Empresa == empresa.Id && c.Id_Estado_Candidatura == 1)
                .CountAsync();

            ViewBag.Notificacoes = await _context.Notificacoes
                .Where(n => n.Id_Utilizador == empresa.Id_Utilizador && !n.Lida)
                .OrderByDescending(n => n.Data_Criacao)
                .Take(5)
                .ToListAsync();

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AprovarCandidatura(int id)
        {
            var empresa = await GetCurrentEmpresa();
            if (empresa == null) return RedirectToAction("Login", "Account");

            var candidatura = await _context.Candidaturas
                .Include(c => c.Oferta)
                .Include(c => c.Aluno)
                .ThenInclude(a => a.Utilizador)
                .FirstOrDefaultAsync(c => c.Id == id && c.Oferta.Id_Empresa == empresa.Id);

            if (candidatura == null) return NotFound();

            candidatura.Id_Estado_Candidatura = 3;
            await _context.SaveChangesAsync();

            _context.Notificacoes.Add(new Notificacao
            {
                Id_Utilizador = candidatura.Aluno.Id_Utilizador,
                Titulo = "Candidatura Aprovada!",
                Mensagem = $"A sua candidatura para '{candidatura.Oferta.Titulo}' foi aprovada pela empresa.",
                Tipo = "success",
                Link = "/Aluno/MinhasCandidaturas"
            });
            await _context.SaveChangesAsync();

            TempData["Success"] = "Candidatura aprovada com sucesso!";
            return RedirectToAction("Candidatos", new { id = candidatura.Id_Oferta });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejeitarCandidatura(int id, string motivo)
        {
            var empresa = await GetCurrentEmpresa();
            if (empresa == null) return RedirectToAction("Login", "Account");

            var candidatura = await _context.Candidaturas
                .Include(c => c.Oferta)
                .Include(c => c.Aluno)
                .ThenInclude(a => a.Utilizador)
                .FirstOrDefaultAsync(c => c.Id == id && c.Oferta.Id_Empresa == empresa.Id);

            if (candidatura == null) return NotFound();

            candidatura.Id_Estado_Candidatura = 4;
            await _context.SaveChangesAsync();

            _context.Notificacoes.Add(new Notificacao
            {
                Id_Utilizador = candidatura.Aluno.Id_Utilizador,
                Titulo = "Candidatura Rejeitada",
                Mensagem = $"A sua candidatura para '{candidatura.Oferta.Titulo}' foi rejeitada. Motivo: {motivo}",
                Tipo = "error",
                Link = "/Aluno/MinhasCandidaturas"
            });
            await _context.SaveChangesAsync();

            TempData["Success"] = "Candidatura rejeitada.";
            return RedirectToAction("Candidatos", new { id = candidatura.Id_Oferta });
        }

        // ==================== CRIAR OFERTA ====================

        public async Task<IActionResult> CriarOferta()
        {
            var empresa = await GetCurrentEmpresa();
            if (empresa == null) return RedirectToAction("Login", "Account");

            ViewBag.TiposOferta = await _context.TipoOfertas.ToListAsync();
            return View(new Oferta());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CriarOferta(Oferta oferta)
        {
            var empresa = await GetCurrentEmpresa();
            if (empresa == null) return RedirectToAction("Login", "Account");

            ValidarOfertaBasica(oferta, ModelState);

            if (oferta.Id_Tipo_Oferta == null || !await _context.TipoOfertas.AnyAsync(t => t.Id == oferta.Id_Tipo_Oferta))
                ModelState.AddModelError(string.Empty, "Selecione um tipo de oferta válido.");

            if (oferta.Data_Expiracao.HasValue && oferta.Data_Expiracao.Value.Date <= DateTime.Today)
                ModelState.AddModelError(string.Empty, "A data de expiração tem de ser posterior à data atual.");

            if (!ModelState.IsValid)
            {
                ViewBag.TiposOferta = await _context.TipoOfertas.ToListAsync();
                return View(oferta);
            }

            oferta.Id_Empresa = empresa.Id;
            oferta.Id_Estado_Oferta = 1;
            oferta.Data_Publicacao = DateTime.Now;

            _context.Ofertas.Add(oferta);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Oferta criada com sucesso!";
            return RedirectToAction("MinhasOfertas");
        }

        // ==================== EDITAR OFERTA ====================

        public async Task<IActionResult> EditarOferta(int? id)
        {
            if (id == null) return NotFound();

            var empresa = await GetCurrentEmpresa();
            var oferta = await _context.Ofertas
                .FirstOrDefaultAsync(o => o.Id == id && o.Id_Empresa == empresa!.Id);

            if (oferta == null) return NotFound();

            ViewBag.TiposOferta = await _context.TipoOfertas.ToListAsync();
            return View(oferta);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarOferta(int id, Oferta oferta)
        {
            var empresa = await GetCurrentEmpresa();
            if (empresa == null) return RedirectToAction("Login", "Account");

            var existing = await _context.Ofertas
                .FirstOrDefaultAsync(o => o.Id == id && o.Id_Empresa == empresa.Id);

            if (existing == null) return NotFound();

            ValidarOfertaBasica(oferta, ModelState);

            if (oferta.Id_Tipo_Oferta == null || !await _context.TipoOfertas.AnyAsync(t => t.Id == oferta.Id_Tipo_Oferta))
                ModelState.AddModelError(string.Empty, "Selecione um tipo de oferta válido.");

            if (oferta.Data_Expiracao.HasValue && oferta.Data_Expiracao.Value.Date <= DateTime.Today)
                ModelState.AddModelError(string.Empty, "A data de expiração tem de ser posterior à data atual.");

            if (!ModelState.IsValid)
            {
                ViewBag.TiposOferta = await _context.TipoOfertas.ToListAsync();
                return View(existing);
            }

            existing.Titulo = oferta.Titulo;
            existing.Descricao = oferta.Descricao;
            existing.Requisitos = oferta.Requisitos;
            existing.Localizacao = oferta.Localizacao;
            existing.Id_Tipo_Oferta = oferta.Id_Tipo_Oferta;
            existing.Data_Expiracao = oferta.Data_Expiracao;
            existing.Latitude = oferta.Latitude;
            existing.Longitude = oferta.Longitude;

            await _context.SaveChangesAsync();
            TempData["Success"] = "Oferta atualizada com sucesso!";
            return RedirectToAction("MinhasOfertas");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarOferta(int id)
        {
            var empresa = await GetCurrentEmpresa();
            var oferta = await _context.Ofertas
                .FirstOrDefaultAsync(o => o.Id == id && o.Id_Empresa == empresa!.Id);

            if (oferta == null) return NotFound();

            var temCandidaturas = await _context.Candidaturas.AnyAsync(c => c.Id_Oferta == id);

            if (temCandidaturas)
            {
                oferta.Id_Estado_Oferta = 4;
                TempData["Success"] = "Oferta desativada (tinha candidaturas associadas).";
            }
            else
            {
                _context.Ofertas.Remove(oferta);
                TempData["Success"] = "Oferta eliminada permanentemente.";
            }

            await _context.SaveChangesAsync();
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

            var candidaturasVisiveis = candidaturas
                .Where(c => c.Avaliacoes?.Any(a => a.Id_Decisao_Avaliacao == 1) == true)
                .ToList();

            ViewBag.Oferta = oferta;
            return View(candidaturasVisiveis);
        }

        // ==================== EMPRESA APROVA/REJEITA CANDIDATO ====================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AprovarCandidatoEmpresa(int id)
        {
            var empresa = await GetCurrentEmpresa();
            if (empresa == null) return RedirectToAction("Login", "Account");

            var candidatura = await _context.Candidaturas
                .Include(c => c.Oferta)
                .Include(c => c.Aluno)
                .ThenInclude(a => a.Utilizador)
                .FirstOrDefaultAsync(c => c.Id == id && c.Oferta.Id_Empresa == empresa.Id);

            if (candidatura == null) return NotFound();

            candidatura.Id_Estado_Candidatura = 3;

            await _context.SaveChangesAsync();

            _context.Notificacoes.Add(new Notificacao
            {
                Id_Utilizador = candidatura.Aluno.Id_Utilizador,
                Titulo = "Candidatura Aprovada pela Empresa!",
                Mensagem = $"A empresa '{empresa.Nome_Empresa}' aprovou a sua candidatura para '{candidatura.Oferta.Titulo}'.",
                Tipo = "success",
                Link = "/Aluno/MinhasCandidaturas"
            });
            await _context.SaveChangesAsync();

            TempData["Success"] = "Candidato aprovado com sucesso!";
            return RedirectToAction("Candidatos", new { id = candidatura.Id_Oferta });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejeitarCandidatoEmpresa(int id, string motivo)
        {
            var empresa = await GetCurrentEmpresa();
            if (empresa == null) return RedirectToAction("Login", "Account");

            var candidatura = await _context.Candidaturas
                .Include(c => c.Oferta)
                .Include(c => c.Aluno)
                .ThenInclude(a => a.Utilizador)
                .FirstOrDefaultAsync(c => c.Id == id && c.Oferta.Id_Empresa == empresa.Id);

            if (candidatura == null) return NotFound();

            candidatura.Id_Estado_Candidatura = 4;

            await _context.SaveChangesAsync();

            _context.Notificacoes.Add(new Notificacao
            {
                Id_Utilizador = candidatura.Aluno.Id_Utilizador,
                Titulo = "Candidatura Rejeitada pela Empresa",
                Mensagem = $"A empresa '{empresa.Nome_Empresa}' rejeitou a sua candidatura para '{candidatura.Oferta.Titulo}'. Motivo: {motivo}",
                Tipo = "error",
                Link = "/Aluno/MinhasCandidaturas"
            });
            await _context.SaveChangesAsync();

            TempData["Success"] = "Candidato rejeitado.";
            return RedirectToAction("Candidatos", new { id = candidatura.Id_Oferta });
        }

        // ==================== PERFIL ====================

        public async Task<IActionResult> Perfil()
        {
            var empresa = await GetCurrentEmpresa();
            if (empresa == null) return RedirectToAction("Login", "Account");

            return View(empresa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AtualizarPerfil(string nomeEmpresa, string nif, string morada, string telefone, string site)
        {
            var empresa = await GetCurrentEmpresa();
            if (empresa == null) return RedirectToAction("Login", "Account");

            empresa.Nome_Empresa = nomeEmpresa;
            empresa.Nif = nif;
            empresa.Morada = morada;
            empresa.Telefone = telefone;
            empresa.Site_Empresa = site;

            empresa.Utilizador.Nome = nomeEmpresa;

            await _context.SaveChangesAsync();
            HttpContext.Session.SetString("UserName", nomeEmpresa);

            TempData["Success"] = "Perfil atualizado com sucesso!";
            return RedirectToAction("Perfil");
        }
    }
}