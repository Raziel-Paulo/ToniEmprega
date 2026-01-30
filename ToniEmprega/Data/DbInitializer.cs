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
            IApplicationBuilder applicationBuilder,
            IServiceProvider services,
            ILogger logger)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider
                    .GetRequiredService<ApplicationDbContext>();

                var userManager = serviceScope.ServiceProvider
                    .GetRequiredService<UserManager<ApplicationUser>>();

                var roleManager = serviceScope.ServiceProvider
                    .GetRequiredService<RoleManager<IdentityRole>>();

                try
                {
                    await context.Database.EnsureCreatedAsync();

                    if (await context.Users.AnyAsync())
                        return;

                    // ✅ CHAMAR A CLASSE CERTA
                    await IdentitySeed.SeedRolesAsync(userManager, roleManager);
                    await IdentitySeed.SeedSuperAdminAsync(userManager, roleManager);
                    await IdentitySeed.SeedBasicUserAsync(userManager, roleManager);
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
