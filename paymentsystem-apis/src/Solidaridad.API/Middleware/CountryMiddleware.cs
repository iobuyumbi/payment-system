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
        if (context.Request.Headers.TryGetValue("X-Country-Code", out var countryCode))
        {
            var countryList = await countryService.GetAllAsync();
            var country = countryList.Any() ? countryList.FirstOrDefault(c => c.Code == countryCode!) : null;
            if (country != null)
            {
                context.Items["CountryId"] = country.Id;
            }
        }

        await _next(context);
    }
}
