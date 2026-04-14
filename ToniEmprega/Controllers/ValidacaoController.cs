// Controllers/ValidacaoController.cs - MODIFICADO
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

            // Se já estiver aprovado, redirecionar
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

            // ✅ MODIFICADO: Buscar validação única do utilizador com documentos
            var validacao = await _context.ValidacoesIdentidade
                .Include(v => v.Documentos)
                .Include(v => v.EstadoValidacaoDocumento)
                .FirstOrDefaultAsync(v => v.Id_Utilizador == userId.Value);

            ViewBag.PrecisaNovaValidacao = utilizador.Id_Estado_Validacao_Utilizador == 3;
            ViewBag.EstadoAtual = utilizador.Id_Estado_Validacao_Utilizador;
            ViewBag.TipoUtilizador = utilizador.Id_Tipo_Utilizador;
            ViewBag.MotivoRejeicao = validacao?.Motivo_Rejeicao;

            // Tipos de documentos disponíveis
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
                return RedirectToAction("Index");
            }

            // Validações de segurança
            var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png" };
            var allowedTypes = new[] { "application/pdf", "image/jpeg", "image/png" };
            const long maxSize = 10 * 1024 * 1024;

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

            // ✅ MODIFICADO: Verificar se já existe validação para este utilizador
            var validacao = await _context.ValidacoesIdentidade
                .FirstOrDefaultAsync(v => v.Id_Utilizador == userId.Value);

            // Se não existir, criar nova validação
            if (validacao == null)
            {
                validacao = new ValidacaoIdentidade
                {
                    Id_Utilizador = userId.Value,
                    Id_Estado_Validacao_Documento = 1, // Pendente
                    Data_Criacao = DateTime.Now
                };
                _context.ValidacoesIdentidade.Add(validacao);
                await _context.SaveChangesAsync();
            }
            else
            {
                // ✅ Se foi rejeitada, volta a pendente ao submeter novo documento
                if (validacao.Id_Estado_Validacao_Documento == 3)
                {
                    validacao.Id_Estado_Validacao_Documento = 1;
                    validacao.Motivo_Rejeicao = null;
                }
            }

            // Guardar ficheiro
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "documentos");
            Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(documento.FileName)}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await documento.CopyToAsync(stream);
            }

            // ✅ Adicionar documento à validação existente
            var docValidacao = new DocumentoValidacao
            {
                Id_Validacao_Identidade = validacao.Id,
                Tipo_Documento = tipoDocumento,
                Nome_Ficheiro = documento.FileName,
                Caminho_Ficheiro = $"/uploads/documentos/{uniqueFileName}",
                Data_Upload = DateTime.Now
            };

            _context.DocumentosValidacao.Add(docValidacao);

            // Atualizar estado do utilizador para pendente
            var utilizador = await _context.Utilizadores.FindAsync(userId.Value);
            if (utilizador != null)
            {
                utilizador.Id_Estado_Validacao_Utilizador = 1;
            }

            await _context.SaveChangesAsync();

            // Notificar administradores
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
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoverDocumento(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue) return RedirectToAction("Login", "Account");

            var documento = await _context.DocumentosValidacao
                .Include(d => d.ValidacaoIdentidade)
                .FirstOrDefaultAsync(d => d.Id == id && d.ValidacaoIdentidade.Id_Utilizador == userId.Value);

            if (documento == null) return NotFound();

            // Verificar se a validação ainda está pendente
            if (documento.ValidacaoIdentidade.Id_Estado_Validacao_Documento != 1)
            {
                TempData["Error"] = "Não pode remover documentos de uma validação já processada.";
                return RedirectToAction("Index");
            }

            // Remover ficheiro físico
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", documento.Caminho_Ficheiro.TrimStart('/'));
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            _context.DocumentosValidacao.Remove(documento);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Documento removido com sucesso.";
            return RedirectToAction("Index");
        }
    }
}