using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToniEmprega.Data;
using ToniEmprega.Models;


namespace ToniEmprega.Data
{
    public class DBInitializer
    {

        public static async Task SeedData(IApplicationBuilder applicationBuilder, IServiceProvider services, ILogger logger)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                if (context == null)
                {
                    logger?.LogError("ApplicationDbContext is not available from the service provider.");
                    return;
                }

                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                try
                {
                    // Make sure the database is created
                    // We already did this in the previous step
                    await context.Database.EnsureCreatedAsync();

                    if (await context.Users.AnyAsync())
                    {
                        //logger.LogInformation("The database was already seeded");
                        return;
                    }

                    //logger.LogInformation("Start seeding the database.");


                    // Identity ...
                    await IdentitySeed.SeedRolesAsync(userManager, roleManager);
                    await IdentitySeed.SeedSuperAdminAsync(userManager, roleManager);
                    await IdentitySeed.SeedBasicUserAsync(userManager, roleManager);


                    // await ModelsSeed.SeedVendasAsync(context, logger);

                    //logger.LogInformation("Finished seeding the database.");
                }
                catch (Exception ex)
                {
                    logger?.LogError(ex, "An error occurred while seeding the database.");
                    throw;
                }
            }
        }
    }
}


