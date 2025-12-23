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
        // Seed adminuser
        var adminUser = new ApplicationUser
        {
            UserName = "adminuser",
            Email = "chauhan.munish1@gmail.com",
            EmailConfirmed = true
        };
        var existingAdminUser = await userManager.FindByEmailAsync(adminUser.Email);
        if (existingAdminUser == null)
        {
            await userManager.CreateAsync(adminUser, "123Pa$$word!");
            await userManager.AddToRoleAsync(adminUser, Roles.Admin.ToString());
        }
        else
        {
            // Ensure existing user has Admin role
            if (!await userManager.IsInRoleAsync(existingAdminUser, Roles.Admin.ToString()))
            {
                await userManager.AddToRoleAsync(existingAdminUser, Roles.Admin.ToString());
            }
        }

        // Seed admin user (from initial request)
        var admin = new ApplicationUser
        {
            UserName = "admin",
            Email = "avivcapital2025@gmail.com",
            EmailConfirmed = true,
            IsLoginEnabled = true,
            IsActive = true
        };
        var existingAdmin = await userManager.FindByEmailAsync(admin.Email);
        if (existingAdmin == null)
        {
            await userManager.CreateAsync(admin, "Aviv2025");
            await userManager.AddToRoleAsync(admin, Roles.Admin.ToString());
        }
        else
        {
            // Update password using ChangePasswordAsync (requires current password) or remove/add password
            // Since we don't know the current password, we'll use a password reset token
            try
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(existingAdmin);
                var resetResult = await userManager.ResetPasswordAsync(existingAdmin, token, "Aviv2025");
                if (!resetResult.Succeeded)
                {
                    Console.WriteLine($"Warning: Could not reset password for admin user: {string.Join(", ", resetResult.Errors.Select(e => e.Description))}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Warning: Could not reset password for admin user (password may already be correct): {ex.Message}");
                // Continue - password might already be correct
            }
            
            if (!await userManager.IsInRoleAsync(existingAdmin, Roles.Admin.ToString()))
            {
                await userManager.AddToRoleAsync(existingAdmin, Roles.Admin.ToString());
            }
            existingAdmin.IsLoginEnabled = true;
            existingAdmin.IsActive = true;
            await userManager.UpdateAsync(existingAdmin);
        }
        
        // Always ensure Admin role has all permissions
        await roleManager.SeedClaimsForSuperAdmin();
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
            // Update password if user already exists using password reset token
            try
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                var resetResult = await userManager.ResetPasswordAsync(user, token, "Super2025");
                if (!resetResult.Succeeded)
                {
                    Console.WriteLine($"Warning: Could not reset password for superadmin user: {string.Join(", ", resetResult.Errors.Select(e => e.Description))}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Warning: Could not reset password for superadmin user (password may already be correct): {ex.Message}");
                // Continue - password might already be correct
            }
            
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
