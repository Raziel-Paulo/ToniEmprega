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
        /// <summary>
        /// Orquestra a migração e o seed das tabelas base, roles e users.
        /// Chamar apenas uma vez (por exemplo do Program.cs).
        /// </summary>
        public static async Task SeedData(IApplicationBuilder app, ILogger logger)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;

                var context = services.GetRequiredService<ApplicationDbContext>();
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                try
                {
                    // Aplica migrations (ou EnsureCreated se preferires).
                    await context.Database.MigrateAsync();

                    // 1) Seed da tabela pai (TiposUtilizador) — obrigatório antes de criar users
                    await IdentitySeed.SeedTiposUtilizadorAsync(context, logger);

                    // 2) Roles (idempotente)
                    await IdentitySeed.SeedRolesAsync(roleManager, logger);

                    // 3) Admins (SuperAdmin + Admin)
                    await IdentitySeed.SeedAdminsAsync(userManager, context, logger);

                    // 4) Basic users
                    await IdentitySeed.SeedBasicUsersAsync(userManager, context, logger);
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
