using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using ToniEmprega.Models;

namespace ToniEmprega.Data
{
    public static class IdentitySeed
    {
        // ---------------------------
        // ROLES
        // ---------------------------
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            foreach (var role in Enum.GetNames(typeof(Enums.Roles)))
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        // ---------------------------
        // SUPER ADMIN + ADMIN
        // ---------------------------
        public static async Task SeedAdminsAsync(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            var tipoSuperAdmin = await context.TiposUtilizador
                .FirstAsync(t => t.Nome == "SuperAdmin");

            var tipoAdmin = await context.TiposUtilizador
                .FirstAsync(t => t.Nome == "Admin");

            await CreateUserIfNotExists(
                userManager,
                "superadmin@gmail.com",
                tipoSuperAdmin.Id,
                new[]
                {
                    Enums.Roles.Basic,
                    Enums.Roles.Moderator,
                    Enums.Roles.Admin,
                    Enums.Roles.SuperAdmin
                });

            await CreateUserIfNotExists(
                userManager,
                "admin@gmail.com",
                tipoAdmin.Id,
                new[]
                {
                    Enums.Roles.Basic,
                    Enums.Roles.Moderator,
                    Enums.Roles.Admin
                });
        }

        // ---------------------------
        // BASIC USERS
        // ---------------------------
        public static async Task SeedBasicUsersAsync(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            var tipoBasic = await context.TiposUtilizador
                .FirstAsync(t => t.Nome == "Basic");

            await CreateUserIfNotExists(
                userManager,
                "client1@gmail.com",
                tipoBasic.Id,
                new[] { Enums.Roles.Basic });

            await CreateUserIfNotExists(
                userManager,
                "client2@gmail.com",
                tipoBasic.Id,
                new[] { Enums.Roles.Basic });
        }

        // ---------------------------
        // HELPER
        // ---------------------------
        private static async Task CreateUserIfNotExists(
            UserManager<ApplicationUser> userManager,
            string email,
            int tipoUtilizadorId,
            Enums.Roles[] roles)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    TipoUtilizadorId = tipoUtilizadorId
                };

                var result = await userManager.CreateAsync(user, "123Pa$$word.");
                if (!result.Succeeded)
                    throw new Exception($"Erro ao criar utilizador {email}");
            }

            foreach (var role in roles)
            {
                if (!await userManager.IsInRoleAsync(user, role.ToString()))
                {
                    await userManager.AddToRoleAsync(user, role.ToString());
                }
            }
        }
    }
}
