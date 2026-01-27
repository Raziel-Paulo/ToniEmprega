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
    public class OfertasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OfertasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Ofertas
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Ofertas.Include(o => o.Empresa).Include(o => o.EstadoOferta).Include(o => o.TipoOferta);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Ofertas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var oferta = await _context.Ofertas
                .Include(o => o.Empresa)
                .Include(o => o.EstadoOferta)
                .Include(o => o.TipoOferta)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (oferta == null)
            {
                return NotFound();
            }

            return View(oferta);
        }

        // GET: Ofertas/Create
        public IActionResult Create()
        {
            ViewData["EmpresaId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["EstadoOfertaId"] = new SelectList(_context.EstadosOferta, "Id", "Nome");
            ViewData["TipoOfertaId"] = new SelectList(_context.TiposOferta, "Id", "Nome");
            return View();
        }

        // POST: Ofertas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Titulo,Descricao,Requisitos,Localizacao,TipoOfertaId,EstadoOfertaId,EmpresaId,DataCriacao,DataExpiracao")] Oferta oferta)
        {
            if (ModelState.IsValid)
            {
                _context.Add(oferta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmpresaId"] = new SelectList(_context.Users, "Id", "Id", oferta.EmpresaId);
            ViewData["EstadoOfertaId"] = new SelectList(_context.EstadosOferta, "Id", "Nome", oferta.EstadoOfertaId);
            ViewData["TipoOfertaId"] = new SelectList(_context.TiposOferta, "Id", "Nome", oferta.TipoOfertaId);
            return View(oferta);
        }

        // GET: Ofertas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var oferta = await _context.Ofertas.FindAsync(id);
            if (oferta == null)
            {
                return NotFound();
            }
            ViewData["EmpresaId"] = new SelectList(_context.Users, "Id", "Id", oferta.EmpresaId);
            ViewData["EstadoOfertaId"] = new SelectList(_context.EstadosOferta, "Id", "Nome", oferta.EstadoOfertaId);
            ViewData["TipoOfertaId"] = new SelectList(_context.TiposOferta, "Id", "Nome", oferta.TipoOfertaId);
            return View(oferta);
        }

        // POST: Ofertas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Descricao,Requisitos,Localizacao,TipoOfertaId,EstadoOfertaId,EmpresaId,DataCriacao,DataExpiracao")] Oferta oferta)
        {
            if (id != oferta.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(oferta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OfertaExists(oferta.Id))
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
            ViewData["EmpresaId"] = new SelectList(_context.Users, "Id", "Id", oferta.EmpresaId);
            ViewData["EstadoOfertaId"] = new SelectList(_context.EstadosOferta, "Id", "Nome", oferta.EstadoOfertaId);
            ViewData["TipoOfertaId"] = new SelectList(_context.TiposOferta, "Id", "Nome", oferta.TipoOfertaId);
            return View(oferta);
        }

        // GET: Ofertas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var oferta = await _context.Ofertas
                .Include(o => o.Empresa)
                .Include(o => o.EstadoOferta)
                .Include(o => o.TipoOferta)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (oferta == null)
            {
                return NotFound();
            }

            return View(oferta);
        }

        // POST: Ofertas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var oferta = await _context.Ofertas.FindAsync(id);
            if (oferta != null)
            {
                _context.Ofertas.Remove(oferta);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OfertaExists(int id)
        {
            return _context.Ofertas.Any(e => e.Id == id);
        }
    }
}
