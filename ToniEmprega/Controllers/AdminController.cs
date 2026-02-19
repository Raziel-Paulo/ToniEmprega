using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using ToniEmprega.Data;
using ToniEmprega.Models;

namespace ToniEmprega.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Dashboard()
        {
            ViewBag.TotalUtilizadores = await _context.Utilizadores.CountAsync();
            ViewBag.TotalOfertas = await _context.Ofertas.CountAsync();
            ViewBag.TotalCandidaturas = await _context.Candidaturas.CountAsync();
            ViewBag.ValidacoesPendentes = await _context.ValidacoesIdentidade
                .CountAsync(v => v.Id_Estado_Validacao_Documento == 1);
            ViewBag.NotificacoesPendentes = await _context.Notificacoes
                .CountAsync(n => !n.Lida);

            // ✅ CORRIGIDO - Gráfico de candidaturas por mês
            var candidaturasRaw = await _context.Candidaturas
                .GroupBy(c => new { c.Data_Candidatura.Year, c.Data_Candidatura.Month })
                .Select(g => new {
                    g.Key.Year,
                    g.Key.Month,
                    Count = g.Count()
                })
                .OrderBy(x => x.Year).ThenBy(x => x.Month)
                .Take(6)
                .ToListAsync();

            // Formatar em memória (client-side)
            ViewBag.CandidaturasPorMes = candidaturasRaw
                .Select(x => new {
                    Mes = $"{x.Month}/{x.Year}",
                    x.Count
                })
                .ToList();

            return View();
        }

        // GESTÃO DE UTILIZADORES
        public async Task<IActionResult> Utilizadores(string? searchString, int? tipoId)
        {
            var utilizadores = _context.Utilizadores
                .Include(u => u.TipoUtilizador)
                .Include(u => u.EstadoValidacao)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
                utilizadores = utilizadores.Where(u => u.Nome.Contains(searchString) || u.Email.Contains(searchString));

            if (tipoId.HasValue)
                utilizadores = utilizadores.Where(u => u.Id_Tipo_Utilizador == tipoId);

            ViewBag.TiposUtilizador = await _context.TipoUtilizadores.ToListAsync();
            ViewBag.SearchString = searchString;
            ViewBag.TipoId = tipoId;

            return View(await utilizadores.OrderByDescending(u => u.Data_Registro).ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AlterarEstadoUtilizador(int id, int novoEstado)
        {
            var user = await _context.Utilizadores.FindAsync(id);
            if (user == null) return NotFound();

            user.Id_Estado_Validacao_Utilizador = novoEstado;
            await _context.SaveChangesAsync();

            var estado = await _context.EstadoValidacaoUtilizadores.FindAsync(novoEstado);
            _context.Notificacoes.Add(new Notificacao
            {
                Id_Utilizador = id,
                Titulo = "Estado da conta alterado",
                Mensagem = $"O seu estado foi alterado para: {estado?.Designacao}",
                Tipo = novoEstado == 2 ? "success" : "warning"
            });
            await _context.SaveChangesAsync();

            TempData["Success"] = "Estado atualizado!";
            return RedirectToAction("Utilizadores");
        }

        // GESTÃO DE VALIDAÇÕES
        public async Task<IActionResult> Validacoes()
        {
            var validacoes = await _context.ValidacoesIdentidade
                .Include(v => v.Utilizador)
                .Include(v => v.TipoValidacao)
                .Include(v => v.EstadoValidacaoDocumento)
                .Where(v => v.Id_Estado_Validacao_Documento == 1)
                .ToListAsync();

            return View(validacoes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AprovarValidacao(int id)
        {
            var validacao = await _context.ValidacoesIdentidade
                .Include(v => v.Utilizador)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (validacao != null)
            {
                validacao.Id_Estado_Validacao_Documento = 2;
                validacao.Data_Validacao = DateTime.Now;
                validacao.Utilizador.Id_Estado_Validacao_Utilizador = 2;

                await _context.SaveChangesAsync();

                _context.Notificacoes.Add(new Notificacao
                {
                    Id_Utilizador = validacao.Id_Utilizador,
                    Titulo = "Documento aprovado!",
                    Mensagem = "A sua validação de identidade foi aprovada. Já pode aceder a todas as funcionalidades.",
                    Tipo = "success",
                    Link = "/Validacao/Index"
                });
                await _context.SaveChangesAsync();

                TempData["Success"] = "Validação aprovada!";
            }

            return RedirectToAction("Validacoes");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejeitarValidacao(int id, string motivo)
        {
            var validacao = await _context.ValidacoesIdentidade
                .Include(v => v.Utilizador)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (validacao != null)
            {
                validacao.Id_Estado_Validacao_Documento = 3;
                validacao.Data_Validacao = DateTime.Now;
                validacao.Utilizador.Id_Estado_Validacao_Utilizador = 3;

                await _context.SaveChangesAsync();

                _context.Notificacoes.Add(new Notificacao
                {
                    Id_Utilizador = validacao.Id_Utilizador,
                    Titulo = "Documento rejeitado",
                    Mensagem = $"Motivo: {motivo}. Por favor, submeta novo documento.",
                    Tipo = "error",
                    Link = "/Validacao/Index"
                });
                await _context.SaveChangesAsync();

                TempData["Success"] = "Validação rejeitada e utilizador notificado.";
            }

            return RedirectToAction("Validacoes");
        }

        // GESTÃO DE OFERTAS
        public async Task<IActionResult> Ofertas(string? searchString, int? estadoId)
        {
            var ofertas = _context.Ofertas
                .Include(o => o.Empresa)
                .Include(o => o.EstadoOferta)
                .Include(o => o.TipoOferta)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
                ofertas = ofertas.Where(o => o.Titulo.Contains(searchString));

            if (estadoId.HasValue)
                ofertas = ofertas.Where(o => o.Id_Estado_Oferta == estadoId);

            ViewBag.EstadosOferta = await _context.EstadoOfertas.ToListAsync();
            return View(await ofertas.OrderByDescending(o => o.Data_Publicacao).ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AlterarEstadoOferta(int id, int novoEstado)
        {
            var oferta = await _context.Ofertas
                .Include(o => o.Empresa)
                .ThenInclude(e => e.Utilizador)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (oferta != null)
            {
                oferta.Id_Estado_Oferta = novoEstado;
                await _context.SaveChangesAsync();

                _context.Notificacoes.Add(new Notificacao
                {
                    Id_Utilizador = oferta.Empresa.Id_Utilizador,
                    Titulo = "Estado da oferta alterado",
                    Mensagem = $"A oferta '{oferta.Titulo}' teve o estado alterado.",
                    Tipo = "info",
                    Link = $"/Empresa/MinhasOfertas"
                });
                await _context.SaveChangesAsync();

                TempData["Success"] = "Estado da oferta atualizado!";
            }

            return RedirectToAction("Ofertas");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarOferta(int id)
        {
            var oferta = await _context.Ofertas.FindAsync(id);
            if (oferta != null)
            {
                _context.Ofertas.Remove(oferta);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Oferta eliminada permanentemente.";
            }
            return RedirectToAction("Ofertas");
        }

        // EXPORTAÇÃO DE RELATÓRIOS
        public async Task<IActionResult> ExportarCandidaturas()
        {
            var candidaturas = await _context.Candidaturas
                .Include(c => c.Aluno).ThenInclude(a => a.Utilizador)
                .Include(c => c.Oferta).ThenInclude(o => o.Empresa)
                .Include(c => c.EstadoCandidatura)
                .ToListAsync();

            var csv = new StringBuilder();
            csv.AppendLine("ID,Aluno,Email,Oferta,Empresa,Data Candidatura,Estado");

            foreach (var c in candidaturas)
            {
                csv.AppendLine($"{c.Id},{c.Aluno?.Utilizador?.Nome},{c.Aluno?.Utilizador?.Email},{c.Oferta?.Titulo},{c.Oferta?.Empresa?.Nome_Empresa},{c.Data_Candidatura:dd/MM/yyyy},{c.EstadoCandidatura?.Designacao}");
            }

            var bytes = Encoding.UTF8.GetBytes(csv.ToString());
            return File(bytes, "text/csv", $"candidaturas_{DateTime.Now:yyyyMMdd}.csv");
        }

        public async Task<IActionResult> ExportarUtilizadores()
        {
            var utilizadores = await _context.Utilizadores
                .Include(u => u.TipoUtilizador)
                .Include(u => u.EstadoValidacao)
                .ToListAsync();

            var csv = new StringBuilder();
            csv.AppendLine("ID,Nome,Email,Tipo,Estado,Data Registo");

            foreach (var u in utilizadores)
            {
                csv.AppendLine($"{u.Id},{u.Nome},{u.Email},{u.TipoUtilizador?.Designacao},{u.EstadoValidacao?.Designacao},{u.Data_Registro:dd/MM/yyyy}");
            }

            var bytes = Encoding.UTF8.GetBytes(csv.ToString());
            return File(bytes, "text/csv", $"utilizadores_{DateTime.Now:yyyyMMdd}.csv");
        }

        // GESTÃO DE TIPOS E ESTADOS
        public async Task<IActionResult> Configuracoes()
        {
            ViewBag.TiposUtilizador = await _context.TipoUtilizadores.ToListAsync();
            ViewBag.TiposOferta = await _context.TipoOfertas.ToListAsync();
            ViewBag.EstadosCandidatura = await _context.EstadoCandidaturas.ToListAsync();
            ViewBag.EstadosOferta = await _context.EstadoOfertas.ToListAsync();
            ViewBag.TiposValidacao = await _context.TipoValidacoes.ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdicionarTipoOferta(string designacao, string descricao)
        {
            _context.TipoOfertas.Add(new TipoOferta
            {
                Designacao = designacao,
                Descricao = descricao
            });
            await _context.SaveChangesAsync();
            TempData["Success"] = "Tipo de oferta adicionado!";
            return RedirectToAction("Configuracoes");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoverTipoOferta(int id)
        {
            var tipo = await _context.TipoOfertas.FindAsync(id);
            if (tipo != null)
            {
                _context.TipoOfertas.Remove(tipo);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Tipo removido!";
            }
            return RedirectToAction("Configuracoes");
        }
    }
}