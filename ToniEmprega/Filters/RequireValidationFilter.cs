using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using ToniEmprega.Data;

namespace ToniEmprega.Filters
{
    public class RequireValidationFilter : IActionFilter
    {
        private readonly ApplicationDbContext _context;

        public RequireValidationFilter(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Ignorar se for página de validação, login, register ou logout
            var controller = context.RouteData.Values["controller"]?.ToString();
            var action = context.RouteData.Values["action"]?.ToString();

            var allowedControllers = new[] { "Validacao", "Account", "Home" };
            var allowedActions = new[] { "Login", "Register", "Logout", "Index", "Privacy", "Error" };

            if (allowedControllers.Contains(controller) || allowedActions.Contains(action))
                return;

            // Verificar se utilizador está autenticado
            var userId = context.HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }

            // Verificar estado de validação na base de dados (não na session!)
            var utilizador = _context.Utilizadores
                .AsNoTracking()
                .FirstOrDefault(u => u.Id == userId.Value);

            if (utilizador == null)
            {
                context.HttpContext.Session.Clear();
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }

            // Se não estiver aprovado (estado 2), redirecionar para validação
            if (utilizador.Id_Estado_Validacao_Utilizador != 2)
            {
                // Guardar a URL tentada para redirecionar depois da validação
                context.HttpContext.Session.SetString("ReturnUrl",
                    context.HttpContext.Request.Path + context.HttpContext.Request.QueryString);

                context.Result = new RedirectToActionResult("Index", "Validacao", new
                {
                    mensagem = "Precisa de validar a sua identidade para aceder a esta página."
                });
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}