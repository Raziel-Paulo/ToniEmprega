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
    public class CandidaturasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CandidaturasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Candidaturas
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Candidaturas.Include(c => c.Aluno).Include(c => c.EstadoCandidatura).Include(c => c.Oferta);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Candidaturas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var candidatura = await _context.Candidaturas
                .Include(c => c.Aluno)
                .Include(c => c.EstadoCandidatura)
                .Include(c => c.Oferta)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (candidatura == null)
            {
                return NotFound();
            }

            return View(candidatura);
        }

        // GET: Candidaturas/Create
        public IActionResult Create()
        {
            ViewData["AlunoId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["EstadoCandidaturaId"] = new SelectList(_context.EstadosCandidatura, "Id", "Nome");
            ViewData["OfertaId"] = new SelectList(_context.Ofertas, "Id", "Descricao");
            return View();
        }

        // POST: Candidaturas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OfertaId,AlunoId,EstadoCandidaturaId,DataSubmissao")] Candidatura candidatura)
        {
            if (ModelState.IsValid)
            {
                _context.Add(candidatura);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AlunoId"] = new SelectList(_context.Users, "Id", "Id", candidatura.AlunoId);
            ViewData["EstadoCandidaturaId"] = new SelectList(_context.EstadosCandidatura, "Id", "Nome", candidatura.EstadoCandidaturaId);
            ViewData["OfertaId"] = new SelectList(_context.Ofertas, "Id", "Descricao", candidatura.OfertaId);
            return View(candidatura);
        }

        // GET: Candidaturas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var candidatura = await _context.Candidaturas.FindAsync(id);
            if (candidatura == null)
            {
                return NotFound();
            }
            ViewData["AlunoId"] = new SelectList(_context.Users, "Id", "Id", candidatura.AlunoId);
            ViewData["EstadoCandidaturaId"] = new SelectList(_context.EstadosCandidatura, "Id", "Nome", candidatura.EstadoCandidaturaId);
            ViewData["OfertaId"] = new SelectList(_context.Ofertas, "Id", "Descricao", candidatura.OfertaId);
            return View(candidatura);
        }

        // POST: Candidaturas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OfertaId,AlunoId,EstadoCandidaturaId,DataSubmissao")] Candidatura candidatura)
        {
            if (id != candidatura.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(candidatura);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CandidaturaExists(candidatura.Id))
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
            ViewData["AlunoId"] = new SelectList(_context.Users, "Id", "Id", candidatura.AlunoId);
            ViewData["EstadoCandidaturaId"] = new SelectList(_context.EstadosCandidatura, "Id", "Nome", candidatura.EstadoCandidaturaId);
            ViewData["OfertaId"] = new SelectList(_context.Ofertas, "Id", "Descricao", candidatura.OfertaId);
            return View(candidatura);
        }

        // GET: Candidaturas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var candidatura = await _context.Candidaturas
                .Include(c => c.Aluno)
                .Include(c => c.EstadoCandidatura)
                .Include(c => c.Oferta)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (candidatura == null)
            {
                return NotFound();
            }

            return View(candidatura);
        }

        // POST: Candidaturas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var candidatura = await _context.Candidaturas.FindAsync(id);
            if (candidatura != null)
            {
                _context.Candidaturas.Remove(candidatura);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CandidaturaExists(int id)
        {
            return _context.Candidaturas.Any(e => e.Id == id);
        }
    }
}
