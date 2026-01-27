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
    public class TipoValidacoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TipoValidacoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TipoValidacoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.TiposValidacao.ToListAsync());
        }

        // GET: TipoValidacoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoValidacao = await _context.TiposValidacao
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tipoValidacao == null)
            {
                return NotFound();
            }

            return View(tipoValidacao);
        }

        // GET: TipoValidacoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoValidacoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome")] TipoValidacao tipoValidacao)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipoValidacao);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoValidacao);
        }

        // GET: TipoValidacoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoValidacao = await _context.TiposValidacao.FindAsync(id);
            if (tipoValidacao == null)
            {
                return NotFound();
            }
            return View(tipoValidacao);
        }

        // POST: TipoValidacoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome")] TipoValidacao tipoValidacao)
        {
            if (id != tipoValidacao.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipoValidacao);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoValidacaoExists(tipoValidacao.Id))
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
            return View(tipoValidacao);
        }

        // GET: TipoValidacoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoValidacao = await _context.TiposValidacao
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tipoValidacao == null)
            {
                return NotFound();
            }

            return View(tipoValidacao);
        }

        // POST: TipoValidacoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tipoValidacao = await _context.TiposValidacao.FindAsync(id);
            if (tipoValidacao != null)
            {
                _context.TiposValidacao.Remove(tipoValidacao);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipoValidacaoExists(int id)
        {
            return _context.TiposValidacao.Any(e => e.Id == id);
        }
    }
}
