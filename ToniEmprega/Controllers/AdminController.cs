// Controllers/AdminController.cs (ATUALIZADO COMPLETO)
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

            return View();
        }

        public async Task<IActionResult> Utilizadores()
        {
            var utilizadores = await _context.Utilizadores
                .Include(u => u.TipoUtilizador)
                .Include(u => u.EstadoValidacao)
                .OrderByDescending(u => u.Data_Registro)
                .ToListAsync();

            return View(utilizadores);
        }

        public async Task<IActionResult> Validacoes()
        {
            var validacoes = await _context.ValidacoesIdentidade
                .Include(v => v.Utilizador)
                .Include(v => v.TipoValidacao)
                .Include(v => v.EstadoValidacaoDocumento)
                .Where(v => v.Id_Estado_Validacao_Documento == 1) // Pendentes
                .ToListAsync();

            return View(validacoes);
        }

        [HttpPost]
        public async Task<IActionResult> AprovarValidacao(int id)
        {
            var validacao = await _context.ValidacoesIdentidade
                .Include(v => v.Utilizador)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (validacao != null)
            {
                validacao.Id_Estado_Validacao_Documento = 2; // Aprovado
                validacao.Data_Validacao = DateTime.Now;

                validacao.Utilizador.Id_Estado_Validacao_Utilizador = 2; // Aprovado

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Validacoes");
        }

        [HttpPost]
        public async Task<IActionResult> RejeitarValidacao(int id)
        {
            var validacao = await _context.ValidacoesIdentidade.FindAsync(id);
            if (validacao != null)
            {
                validacao.Id_Estado_Validacao_Documento = 3; // Rejeitado
                validacao.Data_Validacao = DateTime.Now;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Validacoes");
        }
    }
}