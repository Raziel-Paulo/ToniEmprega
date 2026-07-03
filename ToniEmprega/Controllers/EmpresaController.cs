using Humanizer.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using ToniEmprega.Data;
using ToniEmprega.Models;
using System.Net;
using System.Net.Mail;

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
                .Where(c => c.Oferta.Id_Empresa == empresa.Id
                         && c.Id_Estado_Candidatura == 6  // Aprovada pelo Professor
                         && c.Id_Estado_Candidatura_Empresa == null) // Empresa ainda não decidiu
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
                .Include(o => o.EstadoOferta)
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

            // ✅ NOVO: a empresa só pode alternar entre Ativa (1) e Desativada (4).
            // Estados geridos pelo sistema/admin (Expirada=2, Preenchida=3) não podem
            // ser definidos manualmente pela empresa por aqui.
            if (existing.Id_Estado_Oferta == 1 || existing.Id_Estado_Oferta == 4)
            {
                if (oferta.Id_Estado_Oferta == 1 || oferta.Id_Estado_Oferta == 4)
                {
                    existing.Id_Estado_Oferta = oferta.Id_Estado_Oferta;
                }
            }

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
                .Include(c => c.EstadoCandidaturaEmpresa)
                .Include(c => c.Ficheiros)
                .Include(c => c.Avaliacoes)
                .ThenInclude(a => a.DecisaoAvaliacao)
                .Where(c => c.Id_Oferta == id)
                .ToListAsync();

            // ✅ CORRIGIDO: Mostrar apenas candidaturas APROVADAS PELO PROFESSOR (estado 6)
            // E também as que a empresa já decidiu (para histórico)
            var candidaturasVisiveis = candidaturas
                .Where(c => c.Id_Estado_Candidatura == 6   // Aprovada pelo Professor
                         || c.Id_Estado_Candidatura_Empresa == 3  // Aprovada pela Empresa
                         || c.Id_Estado_Candidatura_Empresa == 4)  // Rejeitada pela Empresa
                .ToList();

            ViewBag.Oferta = oferta;
            return View(candidaturasVisiveis);
        }

        // ==================== EMPRESA APROVA/REJEITA CANDIDATO ====================

        private async Task EnviarEmailAprovacaoAluno(string emailAluno, string nomeAluno, string nomeEmpresa, string tituloOferta, string mensagemPersonalizada)
        {
            var smtpServer = _configuration["Email:SmtpServer"] ?? "smtp.gmail.com";
            var smtpPort = int.Parse(_configuration["Email:SmtpPort"] ?? "587");
            var smtpUser = _configuration["Email:SmtpUser"] ?? string.Empty;
            var smtpPass = _configuration["Email:SmtpPass"] ?? string.Empty;
            var fromEmail = _configuration["Email:FromEmail"] ?? "noreply@toniemprega.pt";

            using var client = new SmtpClient(smtpServer, smtpPort)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(smtpUser, smtpPass)
            };

            var message = new MailMessage
            {
                From = new MailAddress(fromEmail, "ToniEmprega"),
                Subject = $"🎉 Candidatura Aprovada - {nomeEmpresa}",
                IsBodyHtml = true
            };

            message.To.Add(emailAluno);

            // Mensagem do email
            var corpoEmail = $@"
<h2 style='color: #28a745;'>Parabéns, {nomeAluno}!</h2>
<p>A empresa <strong>{nomeEmpresa}</strong> aprovou a sua candidatura para a oferta:</p>
<h3 style='color: #1E90FF;'>{tituloOferta}</h3>
<hr style='border: none; border-top: 1px solid #eee; margin: 1.5rem 0;' />
<h4 style='color: #333;'>Mensagem da empresa:</h4>
<div style='background: #f8f9fa; padding: 1rem; border-radius: 8px; border-left: 4px solid #1E90FF;'>
    <p style='margin: 0; white-space: pre-line;'>{System.Net.WebUtility.HtmlEncode(mensagemPersonalizada)}</p>
</div>
<hr style='border: none; border-top: 1px solid #eee; margin: 1.5rem 0;' />
<p style='color: #666;'>Entre na plataforma ToniEmprega para mais detalhes.</p>
<p style='color: #666;'>Boa sorte! 🍀</p>
<br>
<p style='color: #999; font-size: 0.9rem;'>Atenciosamente,<br><strong>Equipa ToniEmprega</strong></p>";

            message.Body = corpoEmail;

            await client.SendMailAsync(message);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AprovarCandidatoEmpresa(int id, string mensagemEmail)
        {
            var empresa = await GetCurrentEmpresa();
            if (empresa == null) return RedirectToAction("Login", "Account");

            // ✅ VALIDAÇÃO: Email obrigatório
            if (string.IsNullOrWhiteSpace(mensagemEmail) || mensagemEmail.Length < 10)
            {
                TempData["Error"] = "É obrigatório escrever uma mensagem de email (mínimo 10 caracteres).";
                return RedirectToAction("Candidatos", new { id });
            }

            var candidatura = await _context.Candidaturas
                .Include(c => c.Oferta)
                .Include(c => c.Aluno)
                .ThenInclude(a => a.Utilizador)
                .FirstOrDefaultAsync(c => c.Id == id && c.Oferta.Id_Empresa == empresa.Id);

            if (candidatura == null) return NotFound();

            // Aprovar candidatura
            candidatura.Id_Estado_Candidatura_Empresa = 3;
            await _context.SaveChangesAsync();

            // Enviar email ao aluno
            try
            {
                await EnviarEmailAprovacaoAluno(
                    candidatura.Aluno.Utilizador.Email,
                    candidatura.Aluno.Utilizador.Nome,
                    empresa.Nome_Empresa,
                    candidatura.Oferta.Titulo,
                    mensagemEmail
                );
                TempData["Success"] = "Candidatura aprovada e email enviado com sucesso!";
            }
            catch (Exception ex)
            {
                TempData["Warning"] = $"Candidatura aprovada, mas o email não foi enviado: {ex.Message}";
            }

            // Notificar aluno na plataforma
            _context.Notificacoes.Add(new Notificacao
            {
                Id_Utilizador = candidatura.Aluno.Id_Utilizador,
                Titulo = "Candidatura Aprovada pela Empresa!",
                Mensagem = $"A empresa '{empresa.Nome_Empresa}' aprovou a sua candidatura para '{candidatura.Oferta.Titulo}'.",
                Tipo = "success",
                Link = "/Aluno/MinhasCandidaturas"
            });
            await _context.SaveChangesAsync();

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

            // ✅ CORRIGIDO: Usar o campo SEPARADO da empresa
            candidatura.Id_Estado_Candidatura_Empresa = 4; // Rejeitada pela Empresa
            // NÃO alterar Id_Estado_Candidatura (mantém o estado do professor = 6)

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

            TempData["Success"] = "Candidatura rejeitada.";
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