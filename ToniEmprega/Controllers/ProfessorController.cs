// Controllers/ProfessorController.cs
using Microsoft.AspNetCore.Mvc;
using ToniEmprega.Data;

namespace ToniEmprega.Controllers
{
    public class ProfessorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProfessorController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            return View();
        }
    }
}