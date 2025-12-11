using Microsoft.AspNetCore.Identity;
using Solidaridad.Core.Enums;
using Solidaridad.DataAccess.Identity;

namespace Solidaridad.DataAccess.Persistence.Seeding.Permission;

public static class DefaultRoles
{
    public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
        await CreateRoleIfNotExists(roleManager, Roles.SuperAdmin.ToString());
        await CreateRoleIfNotExists(roleManager, Roles.Admin.ToString());
        await CreateRoleIfNotExists(roleManager, Roles.Basic.ToString());
        await CreateRoleIfNotExists(roleManager, Roles.Initiator.ToString());
        await CreateRoleIfNotExists(roleManager, Roles.Reviewer.ToString());
        await CreateRoleIfNotExists(roleManager, Roles.Approver.ToString());
    }

    private static async Task CreateRoleIfNotExists(RoleManager<ApplicationRole> roleManager, string roleName)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new ApplicationRole(roleName));
        }
    }
}
