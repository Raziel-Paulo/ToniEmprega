using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ToniEmprega.Data
{
    public class IdentitySeed
    {
        public static async Task SeedRolesAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            await roleManager.CreateAsync(new IdentityRole(Enums.Roles.SuperAdmin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enums.Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enums.Roles.Moderator.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enums.Roles.Basic.ToString()));
        }
        public static async Task SeedSuperAdminAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Admin and SuoerAdmin User
            var email = "superadmin@gmail.com";
            var defaultUser = new IdentityUser
            {
                UserName = email,              // <-- use email as username
                Email = email,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                var createResult = await userManager.CreateAsync(defaultUser, "123Pa$$word.");
                if (!createResult.Succeeded) return;
                user = await userManager.FindByEmailAsync(email);
            }

            // Ensure roles assigned (idempotent)
            if (!await userManager.IsInRoleAsync(user, Enums.Roles.Basic.ToString()))
                await userManager.AddToRoleAsync(user, Enums.Roles.Basic.ToString());
            if (!await userManager.IsInRoleAsync(user, Enums.Roles.Moderator.ToString()))
                await userManager.AddToRoleAsync(user, Enums.Roles.Moderator.ToString());
            if (!await userManager.IsInRoleAsync(user, Enums.Roles.Admin.ToString()))
                await userManager.AddToRoleAsync(user, Enums.Roles.Admin.ToString());
            if (!await userManager.IsInRoleAsync(user, Enums.Roles.SuperAdmin.ToString()))
                await userManager.AddToRoleAsync(user, Enums.Roles.SuperAdmin.ToString());

            email = "admin@gmail.com";
            defaultUser = new IdentityUser
            {
                UserName = email,              // <-- use email as username
                Email = email,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                var createResult = await userManager.CreateAsync(defaultUser, "123Pa$$word.");
                if (!createResult.Succeeded) return;
                user = await userManager.FindByEmailAsync(email);
            }

            // Ensure roles assigned (idempotent)
            if (!await userManager.IsInRoleAsync(user, Enums.Roles.Basic.ToString()))
                await userManager.AddToRoleAsync(user, Enums.Roles.Basic.ToString());
            if (!await userManager.IsInRoleAsync(user, Enums.Roles.Moderator.ToString()))
                await userManager.AddToRoleAsync(user, Enums.Roles.Moderator.ToString());
            if (!await userManager.IsInRoleAsync(user, Enums.Roles.Admin.ToString()))
                await userManager.AddToRoleAsync(user, Enums.Roles.Admin.ToString());

        }
        public static async Task SeedBasicUserAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var email = "client1@gmail.com";
            var defaultUser = new IdentityUser
            {
                UserName = email,              // <-- use email as username
                Email = email,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                var createResult = await userManager.CreateAsync(defaultUser, "123Pa$$word.");
                if (!createResult.Succeeded) return;
                user = await userManager.FindByEmailAsync(email);
            }

            if (!await userManager.IsInRoleAsync(user, Enums.Roles.Basic.ToString()))
                await userManager.AddToRoleAsync(user, Enums.Roles.Basic.ToString());

            email = "client2@gmail.com";
            defaultUser = new IdentityUser
            {
                UserName = email,              // <-- use email as username
                Email = email,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                var createResult = await userManager.CreateAsync(defaultUser, "123Pa$$word.");
                if (!createResult.Succeeded) return;
                user = await userManager.FindByEmailAsync(email);
            }

            if (!await userManager.IsInRoleAsync(user, Enums.Roles.Basic.ToString()))
                await userManager.AddToRoleAsync(user, Enums.Roles.Basic.ToString());

        }
    }
}
