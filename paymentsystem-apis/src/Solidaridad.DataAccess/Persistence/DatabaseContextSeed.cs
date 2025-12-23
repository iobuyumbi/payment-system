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
       RoleManager<ApplicationRole> _roleManager)
    {
        try
        {
            await DefaultRoles.SeedAsync(userManager, _roleManager);
            await DefaultUsers.SeedBasicUserAsync(userManager, _roleManager);
            await DefaultUsers.SeedSuperAdminAsync(userManager, _roleManager);
            //await DefaultUsers.SeedUserAsync(userManager, _roleManager, "initiator", "initiator@gmail.com", Roles.Initiator.ToString());
            //await DefaultUsers.SeedUserAsync(userManager, _roleManager, "reviewer", "reviewer@gmail.com", Roles.Reviewer.ToString());
            //await DefaultUsers.SeedUserAsync(userManager, _roleManager, "approver", "approver@gmail.com", Roles.Approver.ToString());

            await SeedCountriesAsync(context);
            Console.WriteLine("Finished Seeding Default Data");
            Console.WriteLine("Application Starting");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred seeding the DB: {ex.Message}");
        }
    }

    private static async Task SeedCountriesAsync(DatabaseContext context)
    {
        if (!context.Countries.Any())
        {
            context.Countries.Add(new Solidaridad.Core.Entities.Country
            {
                Id = Guid.NewGuid(),
                CountryName = "Kenya",
                Code = "KE",   // ISO 3166-1 alpha-2
                CurrencyName = "Kenyan Shilling",
                CurrencyPrefix = "KES",
                CurrencySuffix = "", // Set empty string to satisfy not-null constraint
                IsActive = true
            });
            await context.SaveChangesAsync();
        }
    }
}
