// Controllers/ValidacaoController.cs (NOVO)
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

            ViewBag.TiposValidacao = await _context.TipoValidacoes.ToListAsync();
            return View(validacoes);
        }

        [HttpPost]
        public async Task<IActionResult> Submeter(IFormFile documento, int tipoValidacaoId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue) return RedirectToAction("Login", "Account");

            if (documento != null && documento.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "documentos");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + documento.FileName;
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
                    Ficheiro_Prova = "/uploads/documentos/" + uniqueFileName,
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
                TempData["Success"] = "Documento submetido com sucesso! Aguarda validação.";
            }

            return RedirectToAction("Index");
        }
    }
}