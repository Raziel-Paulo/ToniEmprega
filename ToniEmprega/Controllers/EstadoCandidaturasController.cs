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
    public class EstadoCandidaturasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EstadoCandidaturasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: EstadoCandidaturas
        public async Task<IActionResult> Index()
        {
            return View(await _context.EstadosCandidatura.ToListAsync());
        }

        // GET: EstadoCandidaturas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estadoCandidatura = await _context.EstadosCandidatura
                .FirstOrDefaultAsync(m => m.Id == id);
            if (estadoCandidatura == null)
            {
                return NotFound();
            }

            return View(estadoCandidatura);
        }

        // GET: EstadoCandidaturas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EstadoCandidaturas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome")] EstadoCandidatura estadoCandidatura)
        {
            if (ModelState.IsValid)
            {
                _context.Add(estadoCandidatura);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(estadoCandidatura);
        }

        // GET: EstadoCandidaturas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estadoCandidatura = await _context.EstadosCandidatura.FindAsync(id);
            if (estadoCandidatura == null)
            {
                return NotFound();
            }
            return View(estadoCandidatura);
        }

        // POST: EstadoCandidaturas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome")] EstadoCandidatura estadoCandidatura)
        {
            if (id != estadoCandidatura.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(estadoCandidatura);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstadoCandidaturaExists(estadoCandidatura.Id))
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
            return View(estadoCandidatura);
        }

        // GET: EstadoCandidaturas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estadoCandidatura = await _context.EstadosCandidatura
                .FirstOrDefaultAsync(m => m.Id == id);
            if (estadoCandidatura == null)
            {
                return NotFound();
            }

            return View(estadoCandidatura);
        }

        // POST: EstadoCandidaturas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var estadoCandidatura = await _context.EstadosCandidatura.FindAsync(id);
            if (estadoCandidatura != null)
            {
                _context.EstadosCandidatura.Remove(estadoCandidatura);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EstadoCandidaturaExists(int id)
        {
            return _context.EstadosCandidatura.Any(e => e.Id == id);
        }
    }
}
