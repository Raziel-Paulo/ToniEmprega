// Controllers/AdminController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToniEmprega.Data;

namespace ToniEmprega.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Dashboard()
        {
            ViewBag.TotalUtilizadores = await _context.Utilizadores.CountAsync();
            ViewBag.TotalOfertas = await _context.Ofertas.CountAsync();
            ViewBag.TotalCandidaturas = await _context.Candidaturas.CountAsync();
            return View();
        }
    }
}