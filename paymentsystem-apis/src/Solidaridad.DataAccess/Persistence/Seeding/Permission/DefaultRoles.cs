using Microsoft.AspNetCore.Identity;
using Solidaridad.Core.Enums;
using Solidaridad.DataAccess.Identity;

namespace Solidaridad.DataAccess.Persistence.Seeding.Permission;

public static class DefaultRoles
{
    public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
        await roleManager.CreateAsync(new ApplicationRole(Roles.Admin.ToString()));
        await roleManager.CreateAsync(new ApplicationRole(Roles.Initiator.ToString()));
        await roleManager.CreateAsync(new ApplicationRole(Roles.Reviewer.ToString()));
        await roleManager.CreateAsync(new ApplicationRole(Roles.Approver.ToString()));
    }
}
