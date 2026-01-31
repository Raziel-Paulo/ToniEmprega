using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using ToniEmprega.Models;

namespace ToniEmprega.Data
{
    public static class IdentitySeed
    {
        // ---------------------------
        // ROLES
        // ---------------------------
        public static async Task SeedRolesAsync(
            RoleManager<IdentityRole> roleManager,
            ILogger logger)
        {
            foreach (var role in Enum.GetNames(typeof(Enums.Roles)))
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var result = await roleManager.CreateAsync(new IdentityRole(role));
                    if (!result.Succeeded)
                    {
                        throw new Exception($"Erro a criar role {role}");
                    }

                    logger.LogInformation("Role criada: {role}", role);
                }
            }
        }

        // ---------------------------
        // ADMINS
        // ---------------------------
        public static async Task SeedAdminsAsync(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            ILogger logger)
        {
            var tipos = await context.TiposUtilizador
                .Where(t => t.Nome == "SuperAdmin" || t.Nome == "Admin")
                .ToListAsync();

            var superAdminTipo = tipos.First(t => t.Nome == "SuperAdmin");
            var adminTipo = tipos.First(t => t.Nome == "Admin");

            await CreateUserAsync(
                userManager,
                "superadmin@gmail.com",
                superAdminTipo.Id,
                new[] { Enums.Roles.Basic, Enums.Roles.Moderator, Enums.Roles.Admin, Enums.Roles.SuperAdmin },
                logger);

            await CreateUserAsync(
                userManager,
                "admin@gmail.com",
                adminTipo.Id,
                new[] { Enums.Roles.Basic, Enums.Roles.Moderator, Enums.Roles.Admin },
                logger);
        }

        // ---------------------------
        // BASIC USERS
        // ---------------------------
        public static async Task SeedBasicUsersAsync(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            ILogger logger)
        {
            var basicTipo = await context.TiposUtilizador
                .SingleAsync(t => t.Nome == "Basic");

            await CreateUserAsync(
                userManager,
                "client1@gmail.com",
                basicTipo.Id,
                new[] { Enums.Roles.Basic },
                logger);

            await CreateUserAsync(
                userManager,
                "client2@gmail.com",
                basicTipo.Id,
                new[] { Enums.Roles.Basic },
                logger);
        }

        // ---------------------------
        // HELPER
        // ---------------------------
        private static async Task CreateUserAsync(
            UserManager<ApplicationUser> userManager,
            string email,
            int tipoUtilizadorId,
            Enums.Roles[] roles,
            ILogger logger)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                    TipoUtilizadorId = tipoUtilizadorId
                };

                var result = await userManager.CreateAsync(user, "123Pa$$word.");
                if (!result.Succeeded)
                {
                    throw new Exception($"Erro a criar user {email}");
                }

                logger.LogInformation("User criado: {email}", email);
            }

            foreach (var role in roles.Select(r => r.ToString()))
            {
                if (!await userManager.IsInRoleAsync(user, role))
                {
                    await userManager.AddToRoleAsync(user, role);
                    logger.LogInformation("Role {role} atribuída a {email}", role, email);
                }
            }
        }
    }
}
