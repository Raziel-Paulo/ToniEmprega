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
    public class EstadoValidacaoDocumentosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EstadoValidacaoDocumentosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: EstadoValidacaoDocumentos
        public async Task<IActionResult> Index()
        {
            return View(await _context.EstadosValidacaoDocumento.ToListAsync());
        }

        // GET: EstadoValidacaoDocumentos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estadoValidacaoDocumento = await _context.EstadosValidacaoDocumento
                .FirstOrDefaultAsync(m => m.Id == id);
            if (estadoValidacaoDocumento == null)
            {
                return NotFound();
            }

            return View(estadoValidacaoDocumento);
        }

        // GET: EstadoValidacaoDocumentos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EstadoValidacaoDocumentos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome")] EstadoValidacaoDocumento estadoValidacaoDocumento)
        {
            if (ModelState.IsValid)
            {
                _context.Add(estadoValidacaoDocumento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(estadoValidacaoDocumento);
        }

        // GET: EstadoValidacaoDocumentos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estadoValidacaoDocumento = await _context.EstadosValidacaoDocumento.FindAsync(id);
            if (estadoValidacaoDocumento == null)
            {
                return NotFound();
            }
            return View(estadoValidacaoDocumento);
        }

        // POST: EstadoValidacaoDocumentos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome")] EstadoValidacaoDocumento estadoValidacaoDocumento)
        {
            if (id != estadoValidacaoDocumento.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(estadoValidacaoDocumento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstadoValidacaoDocumentoExists(estadoValidacaoDocumento.Id))
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
            return View(estadoValidacaoDocumento);
        }

        // GET: EstadoValidacaoDocumentos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estadoValidacaoDocumento = await _context.EstadosValidacaoDocumento
                .FirstOrDefaultAsync(m => m.Id == id);
            if (estadoValidacaoDocumento == null)
            {
                return NotFound();
            }

            return View(estadoValidacaoDocumento);
        }

        // POST: EstadoValidacaoDocumentos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var estadoValidacaoDocumento = await _context.EstadosValidacaoDocumento.FindAsync(id);
            if (estadoValidacaoDocumento != null)
            {
                _context.EstadosValidacaoDocumento.Remove(estadoValidacaoDocumento);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EstadoValidacaoDocumentoExists(int id)
        {
            return _context.EstadosValidacaoDocumento.Any(e => e.Id == id);
        }
    }
}
