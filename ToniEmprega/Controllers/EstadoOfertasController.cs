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
    public class EstadoOfertasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EstadoOfertasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: EstadoOfertas
        public async Task<IActionResult> Index()
        {
            return View(await _context.EstadosOferta.ToListAsync());
        }

        // GET: EstadoOfertas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estadoOferta = await _context.EstadosOferta
                .FirstOrDefaultAsync(m => m.Id == id);
            if (estadoOferta == null)
            {
                return NotFound();
            }

            return View(estadoOferta);
        }

        // GET: EstadoOfertas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EstadoOfertas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome")] EstadoOferta estadoOferta)
        {
            if (ModelState.IsValid)
            {
                _context.Add(estadoOferta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(estadoOferta);
        }

        // GET: EstadoOfertas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estadoOferta = await _context.EstadosOferta.FindAsync(id);
            if (estadoOferta == null)
            {
                return NotFound();
            }
            return View(estadoOferta);
        }

        // POST: EstadoOfertas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome")] EstadoOferta estadoOferta)
        {
            if (id != estadoOferta.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(estadoOferta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstadoOfertaExists(estadoOferta.Id))
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
            return View(estadoOferta);
        }

        // GET: EstadoOfertas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estadoOferta = await _context.EstadosOferta
                .FirstOrDefaultAsync(m => m.Id == id);
            if (estadoOferta == null)
            {
                return NotFound();
            }

            return View(estadoOferta);
        }

        // POST: EstadoOfertas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var estadoOferta = await _context.EstadosOferta.FindAsync(id);
            if (estadoOferta != null)
            {
                _context.EstadosOferta.Remove(estadoOferta);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EstadoOfertaExists(int id)
        {
            return _context.EstadosOferta.Any(e => e.Id == id);
        }
    }
}
