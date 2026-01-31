using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using ToniEmprega.Models;

namespace ToniEmprega.Data
{
    public static class DBInitializer
    {
        public static async Task SeedAsync(IApplicationBuilder app, ILogger logger)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<ApplicationDbContext>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            try
            {
                // 1️⃣ Base de dados + HasData
                await context.Database.MigrateAsync();

                // 2️⃣ Roles
                await IdentitySeed.SeedRolesAsync(roleManager, logger);

                // 3️⃣ Admins
                await IdentitySeed.SeedAdminsAsync(userManager, context, logger);

                // 4️⃣ Users básicos
                await IdentitySeed.SeedBasicUsersAsync(userManager, context, logger);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro fatal no DBInitializer");
                throw;
            }
        }
    }
}
