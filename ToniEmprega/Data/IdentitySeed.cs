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
        // TIPOSUTILIZADOR (pai das FKs)
        // ---------------------------
        public static async Task SeedTiposUtilizadorAsync(ApplicationDbContext context, ILogger logger = null)
        {
            if (await context.TiposUtilizador.AnyAsync())
            {
                logger?.LogInformation("TiposUtilizador já seeded.");
                return;
            }

            // Ajusta os nomes conforme o teu domínio/enums
            context.TiposUtilizador.AddRange(
                new TipoUtilizador { Nome = "Basic" },
                new TipoUtilizador { Nome = "Moderator" },
                new TipoUtilizador { Nome = "Admin" },
                new TipoUtilizador { Nome = "SuperAdmin" }
            );

            await context.SaveChangesAsync();
            logger?.LogInformation("Seed de TiposUtilizador concluído.");
        }

        // ---------------------------
        // ROLES
        // ---------------------------
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager, ILogger logger = null)
        {
            foreach (var roleName in Enum.GetNames(typeof(Enums.Roles)))
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    var res = await roleManager.CreateAsync(new IdentityRole(roleName));
                    if (!res.Succeeded)
                    {
                        var errors = string.Join(", ", res.Errors.Select(e => e.Description));
                        logger?.LogError("Erro ao criar role {role}: {errors}", roleName, errors);
                        throw new Exception($"Erro ao criar role {roleName}: {errors}");
                    }
                    logger?.LogInformation("Role criada: {role}", roleName);
                }
            }
        }

        // ---------------------------
        // SUPERADMIN + ADMIN
        // ---------------------------
        public static async Task SeedAdminsAsync(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            ILogger logger = null)
        {
            // Garante que existem os tipos necessários
            var tipoSuperAdmin = await context.TiposUtilizador.FirstOrDefaultAsync(t => t.Nome == "SuperAdmin");
            var tipoAdmin = await context.TiposUtilizador.FirstOrDefaultAsync(t => t.Nome == "Admin");

            if (tipoSuperAdmin == null || tipoAdmin == null)
            {
                logger?.LogError("TiposUtilizador necessários (SuperAdmin/Admin) não existem. Executa SeedTiposUtilizador primeiro.");
                throw new Exception("TiposUtilizador 'SuperAdmin' ou 'Admin' não encontrados.");
            }

            await CreateUserIfNotExists(userManager,
                email: "superadmin@gmail.com",
                tipoUtilizadorId: tipoSuperAdmin.Id,
                roles: new[] { Enums.Roles.Basic, Enums.Roles.Moderator, Enums.Roles.Admin, Enums.Roles.SuperAdmin },
                logger: logger);

            await CreateUserIfNotExists(userManager,
                email: "admin@gmail.com",
                tipoUtilizadorId: tipoAdmin.Id,
                roles: new[] { Enums.Roles.Basic, Enums.Roles.Moderator, Enums.Roles.Admin },
                logger: logger);
        }

        // ---------------------------
        // BASIC USERS
        // ---------------------------
        public static async Task SeedBasicUsersAsync(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            ILogger logger = null)
        {
            var tipoBasic = await context.TiposUtilizador.FirstOrDefaultAsync(t => t.Nome == "Basic");
            if (tipoBasic == null)
            {
                logger?.LogError("TipoUtilizador 'Basic' não existe.");
                throw new Exception("TipoUtilizador 'Basic' não encontrado.");
            }

            await CreateUserIfNotExists(userManager,
                email: "client1@gmail.com",
                tipoUtilizadorId: tipoBasic.Id,
                roles: new[] { Enums.Roles.Basic },
                logger: logger);

            await CreateUserIfNotExists(userManager,
                email: "client2@gmail.com",
                tipoUtilizadorId: tipoBasic.Id,
                roles: new[] { Enums.Roles.Basic },
                logger: logger);
        }

        // ---------------------------
        // HELPER
        // ---------------------------
        private static async Task CreateUserIfNotExists(
            UserManager<ApplicationUser> userManager,
            string email,
            int tipoUtilizadorId,
            Enums.Roles[] roles,
            ILogger logger = null)
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
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    logger?.LogError("Erro ao criar user {email}: {errors}", email, errors);
                    throw new Exception($"Erro ao criar utilizador {email}: {errors}");
                }

                logger?.LogInformation("User criado: {email}", email);
            }
            else
            {
                logger?.LogInformation("User já existe: {email}", email);
            }

            // Garantir roles do user (idempotente)
            foreach (var role in roles.Select(r => r.ToString()))
            {
                if (!await userManager.IsInRoleAsync(user, role))
                {
                    var addRoleResult = await userManager.AddToRoleAsync(user, role);
                    if (!addRoleResult.Succeeded)
                    {
                        var errors = string.Join(", ", addRoleResult.Errors.Select(e => e.Description));
                        logger?.LogError("Erro ao adicionar role {role} a {email}: {errors}", role, email, errors);
                        throw new Exception($"Erro ao adicionar role {role} a {email}: {errors}");
                    }
                    logger?.LogInformation("Adicionada role {role} a {email}", role, email);
                }
            }
        }
    }
}
