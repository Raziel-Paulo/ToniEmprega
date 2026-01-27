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
    public class AvaliacaoProfessoresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AvaliacaoProfessoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AvaliacaoProfessores
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.AvaliacoesProfessor.Include(a => a.Candidatura).Include(a => a.Decisao).Include(a => a.Professor);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: AvaliacaoProfessores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var avaliacaoProfessor = await _context.AvaliacoesProfessor
                .Include(a => a.Candidatura)
                .Include(a => a.Decisao)
                .Include(a => a.Professor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (avaliacaoProfessor == null)
            {
                return NotFound();
            }

            return View(avaliacaoProfessor);
        }

        // GET: AvaliacaoProfessores/Create
        public IActionResult Create()
        {
            ViewData["CandidaturaId"] = new SelectList(_context.Candidaturas, "Id", "AlunoId");
            ViewData["DecisaoId"] = new SelectList(_context.DecisoesAvaliacao, "Id", "Nome");
            ViewData["ProfessorId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: AvaliacaoProfessores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CandidaturaId,ProfessorId,DecisaoId,Comentario,DataAvaliacao")] AvaliacaoProfessor avaliacaoProfessor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(avaliacaoProfessor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CandidaturaId"] = new SelectList(_context.Candidaturas, "Id", "AlunoId", avaliacaoProfessor.CandidaturaId);
            ViewData["DecisaoId"] = new SelectList(_context.DecisoesAvaliacao, "Id", "Nome", avaliacaoProfessor.DecisaoId);
            ViewData["ProfessorId"] = new SelectList(_context.Users, "Id", "Id", avaliacaoProfessor.ProfessorId);
            return View(avaliacaoProfessor);
        }

        // GET: AvaliacaoProfessores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var avaliacaoProfessor = await _context.AvaliacoesProfessor.FindAsync(id);
            if (avaliacaoProfessor == null)
            {
                return NotFound();
            }
            ViewData["CandidaturaId"] = new SelectList(_context.Candidaturas, "Id", "AlunoId", avaliacaoProfessor.CandidaturaId);
            ViewData["DecisaoId"] = new SelectList(_context.DecisoesAvaliacao, "Id", "Nome", avaliacaoProfessor.DecisaoId);
            ViewData["ProfessorId"] = new SelectList(_context.Users, "Id", "Id", avaliacaoProfessor.ProfessorId);
            return View(avaliacaoProfessor);
        }

        // POST: AvaliacaoProfessores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CandidaturaId,ProfessorId,DecisaoId,Comentario,DataAvaliacao")] AvaliacaoProfessor avaliacaoProfessor)
        {
            if (id != avaliacaoProfessor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(avaliacaoProfessor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AvaliacaoProfessorExists(avaliacaoProfessor.Id))
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
            ViewData["CandidaturaId"] = new SelectList(_context.Candidaturas, "Id", "AlunoId", avaliacaoProfessor.CandidaturaId);
            ViewData["DecisaoId"] = new SelectList(_context.DecisoesAvaliacao, "Id", "Nome", avaliacaoProfessor.DecisaoId);
            ViewData["ProfessorId"] = new SelectList(_context.Users, "Id", "Id", avaliacaoProfessor.ProfessorId);
            return View(avaliacaoProfessor);
        }

        // GET: AvaliacaoProfessores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var avaliacaoProfessor = await _context.AvaliacoesProfessor
                .Include(a => a.Candidatura)
                .Include(a => a.Decisao)
                .Include(a => a.Professor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (avaliacaoProfessor == null)
            {
                return NotFound();
            }

            return View(avaliacaoProfessor);
        }

        // POST: AvaliacaoProfessores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var avaliacaoProfessor = await _context.AvaliacoesProfessor.FindAsync(id);
            if (avaliacaoProfessor != null)
            {
                _context.AvaliacoesProfessor.Remove(avaliacaoProfessor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AvaliacaoProfessorExists(int id)
        {
            return _context.AvaliacoesProfessor.Any(e => e.Id == id);
        }
    }
}
