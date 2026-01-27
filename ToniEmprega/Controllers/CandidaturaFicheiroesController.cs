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
    public class CandidaturaFicheiroesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CandidaturaFicheiroesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CandidaturaFicheiroes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.CandidaturasFicheiros.Include(c => c.Candidatura);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: CandidaturaFicheiroes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var candidaturaFicheiro = await _context.CandidaturasFicheiros
                .Include(c => c.Candidatura)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (candidaturaFicheiro == null)
            {
                return NotFound();
            }

            return View(candidaturaFicheiro);
        }

        // GET: CandidaturaFicheiroes/Create
        public IActionResult Create()
        {
            ViewData["CandidaturaId"] = new SelectList(_context.Candidaturas, "Id", "AlunoId");
            return View();
        }

        // POST: CandidaturaFicheiroes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NomeFicheiro,Caminho,CandidaturaId")] CandidaturaFicheiro candidaturaFicheiro)
        {
            if (ModelState.IsValid)
            {
                _context.Add(candidaturaFicheiro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CandidaturaId"] = new SelectList(_context.Candidaturas, "Id", "AlunoId", candidaturaFicheiro.CandidaturaId);
            return View(candidaturaFicheiro);
        }

        // GET: CandidaturaFicheiroes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var candidaturaFicheiro = await _context.CandidaturasFicheiros.FindAsync(id);
            if (candidaturaFicheiro == null)
            {
                return NotFound();
            }
            ViewData["CandidaturaId"] = new SelectList(_context.Candidaturas, "Id", "AlunoId", candidaturaFicheiro.CandidaturaId);
            return View(candidaturaFicheiro);
        }

        // POST: CandidaturaFicheiroes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NomeFicheiro,Caminho,CandidaturaId")] CandidaturaFicheiro candidaturaFicheiro)
        {
            if (id != candidaturaFicheiro.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(candidaturaFicheiro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CandidaturaFicheiroExists(candidaturaFicheiro.Id))
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
            ViewData["CandidaturaId"] = new SelectList(_context.Candidaturas, "Id", "AlunoId", candidaturaFicheiro.CandidaturaId);
            return View(candidaturaFicheiro);
        }

        // GET: CandidaturaFicheiroes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var candidaturaFicheiro = await _context.CandidaturasFicheiros
                .Include(c => c.Candidatura)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (candidaturaFicheiro == null)
            {
                return NotFound();
            }

            return View(candidaturaFicheiro);
        }

        // POST: CandidaturaFicheiroes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var candidaturaFicheiro = await _context.CandidaturasFicheiros.FindAsync(id);
            if (candidaturaFicheiro != null)
            {
                _context.CandidaturasFicheiros.Remove(candidaturaFicheiro);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CandidaturaFicheiroExists(int id)
        {
            return _context.CandidaturasFicheiros.Any(e => e.Id == id);
        }
    }
}
