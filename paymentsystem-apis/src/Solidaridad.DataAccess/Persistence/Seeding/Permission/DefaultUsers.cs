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

                var userRole = await roleManager.FindByNameAsync(role);
                await roleManager.AddPermissionClaim(userRole, "Farmers");
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
            
            // Ensure user has all required roles
            if (!await userManager.IsInRoleAsync(user, Roles.Basic.ToString()))
                await userManager.AddToRoleAsync(user, Roles.Basic.ToString());
            if (!await userManager.IsInRoleAsync(user, Roles.Admin.ToString()))
                await userManager.AddToRoleAsync(user, Roles.Admin.ToString());
            if (!await userManager.IsInRoleAsync(user, Roles.SuperAdmin.ToString()))
                await userManager.AddToRoleAsync(user, Roles.SuperAdmin.ToString());
        }
        
        // Always update permissions for admin roles
        await roleManager.SeedClaimsForSuperAdmin();
    }

    private async static Task SeedClaimsForSuperAdmin(this RoleManager<ApplicationRole> roleManager)
    {
        var superAdminRole = await roleManager.FindByNameAsync("SuperAdmin");
        if (superAdminRole != null)
        {
            await roleManager.AddAllPermissionsToRole(superAdminRole);
        }
        
        var adminRole = await roleManager.FindByNameAsync("Admin");
        if (adminRole != null)
        {
            await roleManager.AddAllPermissionsToRole(adminRole);
        }
    }

    private async static Task AddAllPermissionsToRole(this RoleManager<ApplicationRole> roleManager, ApplicationRole role)
    {
        var allClaims = await roleManager.GetClaimsAsync(role);
        var allPermissions = Permissions.GenerateAllPermissions();
        
        foreach (var permission in allPermissions)
        {
            if (!allClaims.Any(a => a.Type == "Permission" && a.Value == permission))
            {
                await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
            }
        }
    }

    public static async Task AddPermissionClaim(this RoleManager<ApplicationRole> roleManager, ApplicationRole role, string module)
    {
        var allClaims = await roleManager.GetClaimsAsync(role);
        
        // Add specific module permissions based on the module name
        switch (module.ToLower())
        {
            case "farmers":
                await AddPermissionIfNotExists(roleManager, role, allClaims, Permissions.Farmers.View);
                await AddPermissionIfNotExists(roleManager, role, allClaims, Permissions.Farmers.Create);
                await AddPermissionIfNotExists(roleManager, role, allClaims, Permissions.Farmers.Edit);
                await AddPermissionIfNotExists(roleManager, role, allClaims, Permissions.Farmers.Delete);
                await AddPermissionIfNotExists(roleManager, role, allClaims, Permissions.Farmers.Import);
                break;
            case "dashboard":
                await AddPermissionIfNotExists(roleManager, role, allClaims, Permissions.Dashboard.View);
                break;
        }
    }
    
    private static async Task AddPermissionIfNotExists(RoleManager<ApplicationRole> roleManager, ApplicationRole role, IEnumerable<Claim> existingClaims, string permission)
    {
        if (!existingClaims.Any(a => a.Type == "Permission" && a.Value == permission))
        {
            await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
        }
    }
}
