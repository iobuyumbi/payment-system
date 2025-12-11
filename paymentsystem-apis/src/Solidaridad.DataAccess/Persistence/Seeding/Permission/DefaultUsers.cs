using Solidaridad.Core.Enums;
using Solidaridad.DataAccess.Identity;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Solidaridad.DataAccess.Persistence.Seeding.Permission;

public static class DefaultUsers
{

    public static async Task SeedUserAsync(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        string username,
        string email,
        string role)
    {
        var defaultUser = new ApplicationUser
        {
            UserName = username,
            Email = email,
            EmailConfirmed = true
        };
        if (userManager.Users.All(u => u.Id != defaultUser.Id))
        {
            var user = await userManager.FindByEmailAsync(defaultUser.Email);
            if (user == null)
            {
                await userManager.CreateAsync(defaultUser, "123Pa$$word!");
                await userManager.AddToRoleAsync(defaultUser, role);

                var initiatorRole = await roleManager.FindByNameAsync(role);
                await roleManager.AddPermissionClaim(initiatorRole, "Farmers");
            }
        }
    }

    public static async Task SeedBasicUserAsync(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager)
    {
        var defaultUser = new ApplicationUser
        {
            UserName = "adminuser",
            Email = "chauhan.munish1@gmail.com",
            EmailConfirmed = true
        };
        if (userManager.Users.All(u => u.Id != defaultUser.Id))
        {
            var user = await userManager.FindByEmailAsync(defaultUser.Email);
            if (user == null)
            {
                await userManager.CreateAsync(defaultUser, "123Pa$$word!");
                await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
            }
        }
    }

    public static async Task SeedSuperAdminAsync(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
        var defaultUser = new ApplicationUser
        {
            UserName = "superadmin",
            Email = "superadmin@gmail.com",
            EmailConfirmed = true
        };
        
        var user = await userManager.FindByEmailAsync(defaultUser.Email);
        if (user == null)
        {
            await userManager.CreateAsync(defaultUser, "Super2025");
            await userManager.AddToRoleAsync(defaultUser, Roles.Basic.ToString());
            await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
            await userManager.AddToRoleAsync(defaultUser, Roles.SuperAdmin.ToString());
        }
        else
        {
            // Update password if user already exists
            await userManager.RemovePasswordAsync(user);
            await userManager.AddPasswordAsync(user, "Super2025");
        }
        
        await roleManager.SeedClaimsForSuperAdmin();
    }

    private async static Task SeedClaimsForSuperAdmin(this RoleManager<ApplicationRole> roleManager)
    {
        var adminRole = await roleManager.FindByNameAsync("SuperAdmin");
        await roleManager.AddPermissionClaim(adminRole, "Farmers");
    }

    public static async Task AddPermissionClaim(this RoleManager<ApplicationRole> roleManager, ApplicationRole role, string module)
    {
        var allClaims = await roleManager.GetClaimsAsync(role);
        var allPermissions = Permissions.GeneratePermissionsForModule(module);
        foreach (var permission in allPermissions)
        {
            if (!allClaims.Any(a => a.Type == "Permission" && a.Value == permission))
            {
                await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
            }
        }
    }
}
