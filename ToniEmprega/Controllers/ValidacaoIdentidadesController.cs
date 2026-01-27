using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToniEmprega.Data;
using ToniEmprega.Models;

namespace ToniEmprega.Controllers
{
    public class ValidacaoIdentidadesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ValidacaoIdentidadesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ValidacaoIdentidades
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ValidacoesIdentidade.Include(v => v.EstadoDocumento).Include(v => v.EstadoUtilizador).Include(v => v.TipoValidacao).Include(v => v.Utilizador);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ValidacaoIdentidades/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var validacaoIdentidade = await _context.ValidacoesIdentidade
                .Include(v => v.EstadoDocumento)
                .Include(v => v.EstadoUtilizador)
                .Include(v => v.TipoValidacao)
                .Include(v => v.Utilizador)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (validacaoIdentidade == null)
            {
                return NotFound();
            }

            return View(validacaoIdentidade);
        }

        // GET: ValidacaoIdentidades/Create
        public IActionResult Create()
        {
            ViewData["EstadoValidacaoDocumentoId"] = new SelectList(_context.EstadosValidacaoDocumento, "Id", "Nome");
            ViewData["EstadoValidacaoUtilizadorId"] = new SelectList(_context.EstadosValidacaoUtilizador, "Id", "Nome");
            ViewData["TipoValidacaoId"] = new SelectList(_context.TiposValidacao, "Id", "Nome");
            ViewData["UtilizadorId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: ValidacaoIdentidades/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UtilizadorId,TipoValidacaoId,CaminhoDocumento,EstadoValidacaoUtilizadorId,EstadoValidacaoDocumentoId,DataSubmissao")] ValidacaoIdentidade validacaoIdentidade)
        {
            if (ModelState.IsValid)
            {
                _context.Add(validacaoIdentidade);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EstadoValidacaoDocumentoId"] = new SelectList(_context.EstadosValidacaoDocumento, "Id", "Nome", validacaoIdentidade.EstadoValidacaoDocumentoId);
            ViewData["EstadoValidacaoUtilizadorId"] = new SelectList(_context.EstadosValidacaoUtilizador, "Id", "Nome", validacaoIdentidade.EstadoValidacaoUtilizadorId);
            ViewData["TipoValidacaoId"] = new SelectList(_context.TiposValidacao, "Id", "Nome", validacaoIdentidade.TipoValidacaoId);
            ViewData["UtilizadorId"] = new SelectList(_context.Users, "Id", "Id", validacaoIdentidade.UtilizadorId);
            return View(validacaoIdentidade);
        }

        // GET: ValidacaoIdentidades/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var validacaoIdentidade = await _context.ValidacoesIdentidade.FindAsync(id);
            if (validacaoIdentidade == null)
            {
                return NotFound();
            }
            ViewData["EstadoValidacaoDocumentoId"] = new SelectList(_context.EstadosValidacaoDocumento, "Id", "Nome", validacaoIdentidade.EstadoValidacaoDocumentoId);
            ViewData["EstadoValidacaoUtilizadorId"] = new SelectList(_context.EstadosValidacaoUtilizador, "Id", "Nome", validacaoIdentidade.EstadoValidacaoUtilizadorId);
            ViewData["TipoValidacaoId"] = new SelectList(_context.TiposValidacao, "Id", "Nome", validacaoIdentidade.TipoValidacaoId);
            ViewData["UtilizadorId"] = new SelectList(_context.Users, "Id", "Id", validacaoIdentidade.UtilizadorId);
            return View(validacaoIdentidade);
        }

        // POST: ValidacaoIdentidades/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UtilizadorId,TipoValidacaoId,CaminhoDocumento,EstadoValidacaoUtilizadorId,EstadoValidacaoDocumentoId,DataSubmissao")] ValidacaoIdentidade validacaoIdentidade)
        {
            if (id != validacaoIdentidade.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(validacaoIdentidade);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ValidacaoIdentidadeExists(validacaoIdentidade.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EstadoValidacaoDocumentoId"] = new SelectList(_context.EstadosValidacaoDocumento, "Id", "Nome", validacaoIdentidade.EstadoValidacaoDocumentoId);
            ViewData["EstadoValidacaoUtilizadorId"] = new SelectList(_context.EstadosValidacaoUtilizador, "Id", "Nome", validacaoIdentidade.EstadoValidacaoUtilizadorId);
            ViewData["TipoValidacaoId"] = new SelectList(_context.TiposValidacao, "Id", "Nome", validacaoIdentidade.TipoValidacaoId);
            ViewData["UtilizadorId"] = new SelectList(_context.Users, "Id", "Id", validacaoIdentidade.UtilizadorId);
            return View(validacaoIdentidade);
        }

        // GET: ValidacaoIdentidades/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var validacaoIdentidade = await _context.ValidacoesIdentidade
                .Include(v => v.EstadoDocumento)
                .Include(v => v.EstadoUtilizador)
                .Include(v => v.TipoValidacao)
                .Include(v => v.Utilizador)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (validacaoIdentidade == null)
            {
                return NotFound();
            }

            return View(validacaoIdentidade);
        }

        // POST: ValidacaoIdentidades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var validacaoIdentidade = await _context.ValidacoesIdentidade.FindAsync(id);
            if (validacaoIdentidade != null)
            {
                _context.ValidacoesIdentidade.Remove(validacaoIdentidade);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ValidacaoIdentidadeExists(int id)
        {
            return _context.ValidacoesIdentidade.Any(e => e.Id == id);
        }
    }
}
