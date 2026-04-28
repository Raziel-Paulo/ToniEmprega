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

        public async Task<IActionResult> Index(string? mensagem)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
                return RedirectToAction("Login", "Account");

            var utilizador = await _context.Utilizadores.FindAsync(userId.Value);
            if (utilizador == null)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Login", "Account");
            }

            if (utilizador.Id_Estado_Validacao_Utilizador == 2)
            {
                var returnUrl = HttpContext.Session.GetString("ReturnUrl");
                HttpContext.Session.Remove("ReturnUrl");

                if (!string.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);

                return utilizador.Id_Tipo_Utilizador switch
                {
                    1 => RedirectToAction("Dashboard", "Aluno"),
                    2 => RedirectToAction("Dashboard", "Professor"),
                    3 => RedirectToAction("Dashboard", "Empresa"),
                    5 => RedirectToAction("Dashboard", "Admin"),
                    _ => RedirectToAction("Index", "Home")
                };
            }

            if (!string.IsNullOrEmpty(mensagem))
                TempData["Warning"] = mensagem;

            var validacao = await _context.ValidacoesIdentidade
                .Include(v => v.Documentos)
                .Include(v => v.EstadoValidacaoDocumento)
                .Where(v => v.Id_Utilizador == userId.Value)
                .OrderByDescending(v => v.Data_Criacao)
                .ThenByDescending(v => v.Id)
                .FirstOrDefaultAsync();

            ViewBag.PrecisaNovaValidacao = utilizador.Id_Estado_Validacao_Utilizador == 3;
            ViewBag.EstadoAtual = utilizador.Id_Estado_Validacao_Utilizador;
            ViewBag.TipoUtilizador = utilizador.Id_Tipo_Utilizador;
            ViewBag.MotivoRejeicao = validacao?.Motivo_Rejeicao;

            ViewBag.TiposDocumento = new List<string>
            {
                "Cartão de Estudante",
                "Bilhete de Identidade",
                "Cartão de Cidadão",
                "Passaporte",
                "Comprovativo de Morada"
            };

            return View(validacao);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmeterDocumento(IFormFile documento, string tipoDocumento)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
                return RedirectToAction("Login", "Account");

            if (documento == null || documento.Length == 0)
            {
                TempData["Error"] = "Selecione um documento.";
                return RedirectToAction(nameof(Index));
            }

            var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png" };
            var allowedTypes = new[] { "application/pdf", "image/jpeg", "image/png" };
            const long maxSize = 10 * 1024 * 1024;

            var ext = Path.GetExtension(documento.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(ext))
            {
                TempData["Error"] = "Formato não suportado. Use PDF, JPG ou PNG.";
                return RedirectToAction(nameof(Index));
            }

            if (!allowedTypes.Contains(documento.ContentType))
            {
                TempData["Error"] = "Tipo de ficheiro inválido.";
                return RedirectToAction(nameof(Index));
            }

            if (documento.Length > maxSize)
            {
                TempData["Error"] = "Ficheiro demasiado grande (máximo 10MB).";
                return RedirectToAction(nameof(Index));
            }

            if (string.IsNullOrWhiteSpace(tipoDocumento))
            {
                TempData["Error"] = "Escolha o tipo de documento.";
                return RedirectToAction(nameof(Index));
            }

            var idTipoValidacao = await ObterIdTipoValidacaoAsync(tipoDocumento);
            if (idTipoValidacao == null)
            {
                TempData["Error"] = "Não foi possível determinar o tipo de validação.";
                return RedirectToAction(nameof(Index));
            }

            var validacao = await _context.ValidacoesIdentidade
                .Include(v => v.Documentos)
                .Where(v => v.Id_Utilizador == userId.Value)
                .OrderByDescending(v => v.Data_Criacao)
                .ThenByDescending(v => v.Id)
                .FirstOrDefaultAsync();

            if (validacao != null && validacao.Id_Estado_Validacao_Documento == 2)
            {
                TempData["Error"] = "A sua validação já foi aprovada e não pode adicionar novos documentos.";
                return RedirectToAction(nameof(Index));
            }

            if (validacao == null)
            {
                validacao = new ValidacaoIdentidade
                {
                    Id_Utilizador = userId.Value,
                    Id_Tipo_Validacao = idTipoValidacao.Value,
                    Id_Estado_Validacao_Documento = 1,
                    Data_Criacao = DateTime.UtcNow
                };

                _context.ValidacoesIdentidade.Add(validacao);
                await _context.SaveChangesAsync();
            }
            else
            {
                validacao.Id_Tipo_Validacao = idTipoValidacao.Value;

                if (validacao.Id_Estado_Validacao_Documento == 3)
                {
                    validacao.Id_Estado_Validacao_Documento = 1;
                    validacao.Motivo_Rejeicao = null;
                }
                else if (validacao.Id_Estado_Validacao_Documento == null)
                {
                    validacao.Id_Estado_Validacao_Documento = 1;
                }
            }

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "documentos");
            Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(documento.FileName)}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await documento.CopyToAsync(stream);
            }

            var docValidacao = new DocumentoValidacao
            {
                Id_Validacao_Identidade = validacao.Id,
                Tipo_Documento = tipoDocumento.Trim(),
                Nome_Ficheiro = documento.FileName,
                Caminho_Ficheiro = $"/uploads/documentos/{uniqueFileName}",
                Data_Upload = DateTime.UtcNow
            };

            _context.DocumentosValidacao.Add(docValidacao);

            var utilizador = await _context.Utilizadores.FindAsync(userId.Value);
            if (utilizador != null)
                utilizador.Id_Estado_Validacao_Utilizador = 1;

            await _context.SaveChangesAsync();

            var admins = await _context.Admins.Include(a => a.Utilizador).ToListAsync();
            foreach (var admin in admins)
            {
                _context.Notificacoes.Add(new Notificacao
                {
                    Id_Utilizador = admin.Id_Utilizador,
                    Titulo = "Novo documento para validar",
                    Mensagem = $"{utilizador?.Nome} submeteu {tipoDocumento}.",
                    Tipo = "warning",
                    Link = "/Admin/Validacoes"
                });
            }

            await _context.SaveChangesAsync();

            TempData["Success"] = "Documento adicionado com sucesso! Aguarde aprovação.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoverDocumento(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
                return RedirectToAction("Login", "Account");

            var documento = await _context.DocumentosValidacao
                .Include(d => d.ValidacaoIdentidade)
                .FirstOrDefaultAsync(d => d.Id == id && d.ValidacaoIdentidade.Id_Utilizador == userId.Value);

            if (documento == null)
                return NotFound();

            if (documento.ValidacaoIdentidade.Id_Estado_Validacao_Documento == 2)
            {
                TempData["Error"] = "Não pode remover documentos de uma validação já aprovada.";
                return RedirectToAction(nameof(Index));
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", documento.Caminho_Ficheiro.TrimStart('/'));
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            var totalDocumentos = await _context.DocumentosValidacao
                .CountAsync(d => d.Id_Validacao_Identidade == documento.Id_Validacao_Identidade);

            _context.DocumentosValidacao.Remove(documento);

            if (totalDocumentos <= 1)
            {
                _context.ValidacoesIdentidade.Remove(documento.ValidacaoIdentidade);
            }

            await _context.SaveChangesAsync();

            TempData["Success"] = "Documento removido com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        private async Task<int?> ObterIdTipoValidacaoAsync(string tipoDocumento)
        {
            var tipoNormalizado = tipoDocumento.Trim().ToLowerInvariant();

            var tipo = await _context.TipoValidacoes
                .FirstOrDefaultAsync(t => t.Designacao.ToLower() == tipoNormalizado);

            if (tipo != null)
                return tipo.Id;

            return await _context.TipoValidacoes
                .OrderBy(t => t.Id)
                .Select(t => (int?)t.Id)
                .FirstOrDefaultAsync();
        }
    }
}