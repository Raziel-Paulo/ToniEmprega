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
    public class TipoUtilizadoresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TipoUtilizadoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TipoUtilizadores
        public async Task<IActionResult> Index()
        {
            return View(await _context.TiposUtilizador.ToListAsync());
        }

        // GET: TipoUtilizadores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoUtilizador = await _context.TiposUtilizador
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tipoUtilizador == null)
            {
                return NotFound();
            }

            return View(tipoUtilizador);
        }

        // GET: TipoUtilizadores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoUtilizadores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome")] TipoUtilizador tipoUtilizador)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipoUtilizador);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoUtilizador);
        }

        // GET: TipoUtilizadores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoUtilizador = await _context.TiposUtilizador.FindAsync(id);
            if (tipoUtilizador == null)
            {
                return NotFound();
            }
            return View(tipoUtilizador);
        }

        // POST: TipoUtilizadores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome")] TipoUtilizador tipoUtilizador)
        {
            if (id != tipoUtilizador.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipoUtilizador);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoUtilizadorExists(tipoUtilizador.Id))
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
            return View(tipoUtilizador);
        }

        // GET: TipoUtilizadores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoUtilizador = await _context.TiposUtilizador
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tipoUtilizador == null)
            {
                return NotFound();
            }

            return View(tipoUtilizador);
        }

        // POST: TipoUtilizadores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tipoUtilizador = await _context.TiposUtilizador.FindAsync(id);
            if (tipoUtilizador != null)
            {
                _context.TiposUtilizador.Remove(tipoUtilizador);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipoUtilizadorExists(int id)
        {
            return _context.TiposUtilizador.Any(e => e.Id == id);
        }
    }
}
