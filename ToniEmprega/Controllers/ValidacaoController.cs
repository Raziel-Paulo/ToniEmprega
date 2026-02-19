using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToniEmprega.Data;
using ToniEmprega.Models;

namespace ToniEmprega.Controllers
{
    public class ValidacaoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ValidacaoController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue) return RedirectToAction("Login", "Account");

            var validacoes = await _context.ValidacoesIdentidade
                .Include(v => v.TipoValidacao)
                .Include(v => v.EstadoValidacaoDocumento)
                .Where(v => v.Id_Utilizador == userId.Value)
                .OrderByDescending(v => v.Id)
                .ToListAsync();

            var utilizador = await _context.Utilizadores.FindAsync(userId.Value);
            ViewBag.PrecisaNovaValidacao = utilizador?.Id_Estado_Validacao_Utilizador == 3; // Rejeitado
            ViewBag.EstadoAtual = utilizador?.Id_Estado_Validacao_Utilizador;

            ViewBag.TiposValidacao = await _context.TipoValidacoes.ToListAsync();
            return View(validacoes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submeter(IFormFile documento, int tipoValidacaoId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue) return RedirectToAction("Login", "Account");

            if (documento == null || documento.Length == 0)
            {
                TempData["Error"] = "Selecione um documento.";
                return RedirectToAction("Index");
            }

            // Validações de segurança
            var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png" };
            var allowedTypes = new[] { "application/pdf", "image/jpeg", "image/png" };
            const long maxSize = 10 * 1024 * 1024; // 10MB

            var ext = Path.GetExtension(documento.FileName).ToLower();
            if (!allowedExtensions.Contains(ext))
            {
                TempData["Error"] = "Formato não suportado. Use PDF, JPG ou PNG.";
                return RedirectToAction("Index");
            }

            if (!allowedTypes.Contains(documento.ContentType))
            {
                TempData["Error"] = "Tipo de ficheiro inválido.";
                return RedirectToAction("Index");
            }

            if (documento.Length > maxSize)
            {
                TempData["Error"] = "Ficheiro demasiado grande (máximo 10MB).";
                return RedirectToAction("Index");
            }

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "documentos");
            Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(documento.FileName)}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await documento.CopyToAsync(stream);
            }

            var validacao = new ValidacaoIdentidade
            {
                Id_Utilizador = userId.Value,
                Id_Tipo_Validacao = tipoValidacaoId,
                Id_Estado_Validacao_Documento = 1, // Pendente
                Ficheiro_Prova = $"/uploads/documentos/{uniqueFileName}",
                Data_Validacao = null
            };

            _context.ValidacoesIdentidade.Add(validacao);

            // Atualizar estado do utilizador para pendente
            var utilizador = await _context.Utilizadores.FindAsync(userId.Value);
            if (utilizador != null)
            {
                utilizador.Id_Estado_Validacao_Utilizador = 1;
            }

            await _context.SaveChangesAsync();

            // Notificar administradores (simplificado - na prática, teria lista de admins)
            var admins = await _context.Admins
                .Include(a => a.Utilizador)
                .ToListAsync();

            foreach (var admin in admins)
            {
                _context.Notificacoes.Add(new Notificacao
                {
                    Id_Utilizador = admin.Id_Utilizador,
                    Titulo = "Nova validação pendente",
                    Mensagem = $"{utilizador?.Nome} submeteu documento para validação.",
                    Tipo = "warning",
                    Link = "/Admin/Validacoes"
                });
            }
            await _context.SaveChangesAsync();

            TempData["Success"] = "Documento submetido com sucesso! Aguarde validação.";
            return RedirectToAction("Index");
        }
    }
}