using Solidaridad.Application.Services;

namespace Solidaridad.API.Middleware;

public class CountryMiddleware
{
    private readonly RequestDelegate _next;

    public CountryMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ICountryService countryService)
    {
        var code = context.Request.Headers.TryGetValue("X-Country-Code", out var countryCode)
            ? countryCode.ToString().Trim()
            : "KE";

        try
        {
            var countryList = await countryService.GetAllAsync();
            Console.WriteLine($"CountryMiddleware: Looking for country code: {code}");
            Console.WriteLine($"CountryMiddleware: Found {countryList?.Count() ?? 0} countries in database");

            if (countryList != null && countryList.Any())
            {
                Console.WriteLine($"CountryMiddleware: Available country codes: {string.Join(", ", countryList.Select(c => c.Code))}");
                var country = countryList.FirstOrDefault(c => string.Equals(c.Code?.Trim(), code, StringComparison.OrdinalIgnoreCase));
                if (country != null)
                {
                    context.Items["CountryId"] = country.Id;
                    Console.WriteLine($"CountryMiddleware: Found country {country.CountryName} with ID {country.Id}");
                }
                else
                {
                    Console.WriteLine($"CountryMiddleware: Country with code '{code}' not found");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in CountryMiddleware: {ex.Message}");
        }

        await _next(context);
    }
}
