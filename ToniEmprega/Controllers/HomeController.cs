using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using ToniEmprega.Models; // ajusta namespace ao teu projecto


namespace ToniEmprega.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // Em produção, substitui por uma chamada ao serviço / repo
            var userRole = User.Claims.FirstOrDefault(c => c.Type == "role")?.Value ?? "UtilizadorNormal";


            var vm = new HomeViewModel
            {
                UserName = User.Identity?.Name ?? "Utilizador",
                Role = userRole,
                Snapshot = new SnapshotViewModel
                {
                    MyApplications = 3,
                    MatchingOffers = 12,
                    PendingDocuments = 1
                },
                Offers = SampleData.GetOffers(),
                Applications = SampleData.GetApplications()
            };


            return View(vm);
        }


        [HttpPost]
        public IActionResult Apply(int offerId)
        {
            // Aqui irias validar se o utilizador tem permissão (ex.: validado)
            // Simular resposta
            return Json(new { status = "ok", message = "Candidatura enviada com sucesso!" });
        }
    }
}