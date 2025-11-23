using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Solidaridad.Core.Enums;
using Solidaridad.DataAccess.Identity;
using Solidaridad.DataAccess.Persistence.Seeding.Permission;

namespace Solidaridad.DataAccess.Persistence;

public static class DatabaseContextSeed
{
    public static async Task SeedDatabaseAsync(DatabaseContext context,
       UserManager<ApplicationUser> userManager,
       RoleManager<ApplicationRole> _roleManager,
       ILogger logger)
    {
        try
        {
            await DefaultRoles.SeedAsync(userManager, _roleManager);
            await DefaultUsers.SeedBasicUserAsync(userManager, _roleManager);
            //await DefaultUsers.SeedSuperAdminAsync(userManager, _roleManager);
            //await DefaultUsers.SeedUserAsync(userManager, _roleManager, "initiator", "initiator@gmail.com", Roles.Initiator.ToString());
            //await DefaultUsers.SeedUserAsync(userManager, _roleManager, "reviewer", "reviewer@gmail.com", Roles.Reviewer.ToString());
            //await DefaultUsers.SeedUserAsync(userManager, _roleManager, "approver", "approver@gmail.com", Roles.Approver.ToString());

            logger.LogInformation("Finished Seeding Default Data");
            logger.LogInformation("Application Starting");
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "An error occurred seeding the DB");
        }
    }
}
