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
    public class DecisaoAvaliacoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DecisaoAvaliacoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DecisaoAvaliacoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.DecisoesAvaliacao.ToListAsync());
        }

        // GET: DecisaoAvaliacoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var decisaoAvaliacao = await _context.DecisoesAvaliacao
                .FirstOrDefaultAsync(m => m.Id == id);
            if (decisaoAvaliacao == null)
            {
                return NotFound();
            }

            return View(decisaoAvaliacao);
        }

        // GET: DecisaoAvaliacoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DecisaoAvaliacoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome")] DecisaoAvaliacao decisaoAvaliacao)
        {
            if (ModelState.IsValid)
            {
                _context.Add(decisaoAvaliacao);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(decisaoAvaliacao);
        }

        // GET: DecisaoAvaliacoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var decisaoAvaliacao = await _context.DecisoesAvaliacao.FindAsync(id);
            if (decisaoAvaliacao == null)
            {
                return NotFound();
            }
            return View(decisaoAvaliacao);
        }

        // POST: DecisaoAvaliacoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome")] DecisaoAvaliacao decisaoAvaliacao)
        {
            if (id != decisaoAvaliacao.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(decisaoAvaliacao);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DecisaoAvaliacaoExists(decisaoAvaliacao.Id))
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
            return View(decisaoAvaliacao);
        }

        // GET: DecisaoAvaliacoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var decisaoAvaliacao = await _context.DecisoesAvaliacao
                .FirstOrDefaultAsync(m => m.Id == id);
            if (decisaoAvaliacao == null)
            {
                return NotFound();
            }

            return View(decisaoAvaliacao);
        }

        // POST: DecisaoAvaliacoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var decisaoAvaliacao = await _context.DecisoesAvaliacao.FindAsync(id);
            if (decisaoAvaliacao != null)
            {
                _context.DecisoesAvaliacao.Remove(decisaoAvaliacao);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DecisaoAvaliacaoExists(int id)
        {
            return _context.DecisoesAvaliacao.Any(e => e.Id == id);
        }
    }
}
