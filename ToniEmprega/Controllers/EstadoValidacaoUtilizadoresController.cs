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
    public class EstadoValidacaoUtilizadoresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EstadoValidacaoUtilizadoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: EstadoValidacaoUtilizadores
        public async Task<IActionResult> Index()
        {
            return View(await _context.EstadosValidacaoUtilizador.ToListAsync());
        }

        // GET: EstadoValidacaoUtilizadores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estadoValidacaoUtilizador = await _context.EstadosValidacaoUtilizador
                .FirstOrDefaultAsync(m => m.Id == id);
            if (estadoValidacaoUtilizador == null)
            {
                return NotFound();
            }

            return View(estadoValidacaoUtilizador);
        }

        // GET: EstadoValidacaoUtilizadores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EstadoValidacaoUtilizadores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome")] EstadoValidacaoUtilizador estadoValidacaoUtilizador)
        {
            if (ModelState.IsValid)
            {
                _context.Add(estadoValidacaoUtilizador);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(estadoValidacaoUtilizador);
        }

        // GET: EstadoValidacaoUtilizadores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estadoValidacaoUtilizador = await _context.EstadosValidacaoUtilizador.FindAsync(id);
            if (estadoValidacaoUtilizador == null)
            {
                return NotFound();
            }
            return View(estadoValidacaoUtilizador);
        }

        // POST: EstadoValidacaoUtilizadores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome")] EstadoValidacaoUtilizador estadoValidacaoUtilizador)
        {
            if (id != estadoValidacaoUtilizador.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(estadoValidacaoUtilizador);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstadoValidacaoUtilizadorExists(estadoValidacaoUtilizador.Id))
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
            return View(estadoValidacaoUtilizador);
        }

        // GET: EstadoValidacaoUtilizadores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estadoValidacaoUtilizador = await _context.EstadosValidacaoUtilizador
                .FirstOrDefaultAsync(m => m.Id == id);
            if (estadoValidacaoUtilizador == null)
            {
                return NotFound();
            }

            return View(estadoValidacaoUtilizador);
        }

        // POST: EstadoValidacaoUtilizadores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var estadoValidacaoUtilizador = await _context.EstadosValidacaoUtilizador.FindAsync(id);
            if (estadoValidacaoUtilizador != null)
            {
                _context.EstadosValidacaoUtilizador.Remove(estadoValidacaoUtilizador);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EstadoValidacaoUtilizadorExists(int id)
        {
            return _context.EstadosValidacaoUtilizador.Any(e => e.Id == id);
        }
    }
}
