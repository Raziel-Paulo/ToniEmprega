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
    public class DBInitializer
    {
        public static async Task SeedData(
            IApplicationBuilder app,
            ILogger logger)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;

                var context = services.GetRequiredService<ApplicationDbContext>();
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                try
                {
                    await context.Database.MigrateAsync();

                    // 1️⃣ Roles
                    await IdentitySeed.SeedRolesAsync(roleManager);

                    // 2️⃣ Admins
                    await IdentitySeed.SeedAdminsAsync(userManager, context);

                    // 3️⃣ Users básicos
                    await IdentitySeed.SeedBasicUsersAsync(userManager, context);
                }
                catch (Exception ex)
                {
                    logger?.LogError(ex, "Erro ao fazer seed da base de dados");
                    throw;
                }
            }
        }
    }
}
