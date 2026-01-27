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
    public class TipoOfertasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TipoOfertasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TipoOfertas
        public async Task<IActionResult> Index()
        {
            return View(await _context.TiposOferta.ToListAsync());
        }

        // GET: TipoOfertas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoOferta = await _context.TiposOferta
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tipoOferta == null)
            {
                return NotFound();
            }

            return View(tipoOferta);
        }

        // GET: TipoOfertas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoOfertas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome")] TipoOferta tipoOferta)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipoOferta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoOferta);
        }

        // GET: TipoOfertas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoOferta = await _context.TiposOferta.FindAsync(id);
            if (tipoOferta == null)
            {
                return NotFound();
            }
            return View(tipoOferta);
        }

        // POST: TipoOfertas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome")] TipoOferta tipoOferta)
        {
            if (id != tipoOferta.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipoOferta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoOfertaExists(tipoOferta.Id))
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
            return View(tipoOferta);
        }

        // GET: TipoOfertas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoOferta = await _context.TiposOferta
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tipoOferta == null)
            {
                return NotFound();
            }

            return View(tipoOferta);
        }

        // POST: TipoOfertas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tipoOferta = await _context.TiposOferta.FindAsync(id);
            if (tipoOferta != null)
            {
                _context.TiposOferta.Remove(tipoOferta);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipoOfertaExists(int id)
        {
            return _context.TiposOferta.Any(e => e.Id == id);
        }
    }
}
