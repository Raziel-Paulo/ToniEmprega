using Humanizer.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToniEmprega.Data;
using ToniEmprega.Models;

namespace ToniEmprega.Controllers
{
    public class EmpresaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public EmpresaController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;  
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

            // ✅ CORREÇÃO: Buscar ofertas com candidaturas incluídas
            var ofertas = await _context.Ofertas
                .Include(o => o.TipoOferta)
                .Include(o => o.EstadoOferta)
                .Include(o => o.Candidaturas)  // ✅ Garantir que candidaturas são carregadas
                .ThenInclude(c => c.Aluno)      // ✅ Incluir aluno para mostrar nome se necessário
                .Where(o => o.Id_Empresa == empresa.Id)
                .OrderByDescending(o => o.Data_Publicacao)
                .Take(5)
                .ToListAsync();

            // ✅ CORREÇÃO: Contagem correta de candidaturas
            ViewBag.TotalOfertas = await _context.Ofertas
                .CountAsync(o => o.Id_Empresa == empresa.Id);

            ViewBag.OfertasAtivas = await _context.Ofertas
                .CountAsync(o => o.Id_Empresa == empresa.Id && o.Id_Estado_Oferta == 1);

            // ✅ CORREÇÃO: Contar candidaturas diretamente da tabela Candidaturas
            ViewBag.TotalCandidaturas = await _context.Candidaturas
                .Where(c => c.Oferta.Id_Empresa == empresa.Id)
                .CountAsync();

            ViewBag.CandidaturasPendentes = await _context.Candidaturas
                .Where(c => c.Oferta.Id_Empresa == empresa.Id && c.Id_Estado_Candidatura == 1)
                .CountAsync();

            // Notificações
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

        // APROVAR CANDIDATURA
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

            // Atualizar estado para Aprovada (3)
            candidatura.Id_Estado_Candidatura = 3;
            await _context.SaveChangesAsync();

            // Notificar aluno
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

        // REJEITAR CANDIDATURA
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

            // Atualizar estado para Rejeitada (4)
            candidatura.Id_Estado_Candidatura = 4;
            await _context.SaveChangesAsync();

            // Notificar aluno
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

        public async Task<IActionResult> CriarOferta()
        {
            var empresa = await GetCurrentEmpresa();
            if (empresa == null) return RedirectToAction("Login", "Account");

            ViewBag.TiposOferta = await _context.TipoOfertas.ToListAsync();

            // ✅ NOVO: Passar chave do Google Maps para a view
            ViewBag.GoogleMapsKey = _configuration["GoogleMaps:ApiKey"] ?? "";

            // Retornar modelo vazio em vez de null
            return View(new Oferta());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CriarOferta(Oferta oferta)
        {
            var empresa = await GetCurrentEmpresa();
            if (empresa == null) return RedirectToAction("Login", "Account");

            // ✅ NOVO: Validações adicionais
            if (string.IsNullOrWhiteSpace(oferta.Titulo) || oferta.Titulo.Length < 5)
            {
                ModelState.AddModelError("", "O título deve ter pelo menos 5 caracteres.");
            }

            if (string.IsNullOrWhiteSpace(oferta.Descricao) || oferta.Descricao.Length < 20)
            {
                ModelState.AddModelError("", "A descrição deve ter pelo menos 20 caracteres.");
            }

            if (string.IsNullOrWhiteSpace(oferta.Requisitos) || oferta.Requisitos.Length < 10)
            {
                ModelState.AddModelError("", "Os requisitos devem ter pelo menos 10 caracteres.");
            }

            if (string.IsNullOrWhiteSpace(oferta.Localizacao) || oferta.Localizacao.Length < 5)
            {
                ModelState.AddModelError("", "A localização deve ter pelo menos 5 caracteres.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.TiposOferta = await _context.TipoOfertas.ToListAsync();
                ViewBag.GoogleMapsKey = _configuration["GoogleMaps:ApiKey"] ?? "";
                return View(oferta);
            }

            // Preencher campos automáticos
            oferta.Id_Empresa = empresa.Id;
            oferta.Id_Estado_Oferta = 1; // Ativa
            oferta.Data_Publicacao = DateTime.Now;

            _context.Ofertas.Add(oferta);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Oferta criada com sucesso!";
            return RedirectToAction("MinhasOfertas");
        }

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

            existing.Titulo = oferta.Titulo;
            existing.Descricao = oferta.Descricao;
            existing.Requisitos = oferta.Requisitos;
            existing.Localizacao = oferta.Localizacao;
            existing.Id_Tipo_Oferta = oferta.Id_Tipo_Oferta;
            existing.Data_Expiracao = oferta.Data_Expiracao;

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
                oferta.Id_Estado_Oferta = 4; // Desativada
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

            ViewBag.Oferta = oferta;
            return View(candidaturas);
        }

        // PERFIL DA EMPRESA
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

            // Atualizar também o nome do utilizador
            empresa.Utilizador.Nome = nomeEmpresa;

            await _context.SaveChangesAsync();
            HttpContext.Session.SetString("UserName", nomeEmpresa);

            TempData["Success"] = "Perfil atualizado com sucesso!";
            return RedirectToAction("Perfil");
        }
    }
}