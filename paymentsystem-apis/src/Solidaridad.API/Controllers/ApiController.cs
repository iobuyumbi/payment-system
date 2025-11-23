using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Security.Claims;

namespace Solidaridad.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApiController : ControllerBase
{
    protected string? CountryCode
    {
        get
        {
            if (Request?.Headers.TryGetValue("X-Country-Code", out StringValues values) == true)
            {
                return values.FirstOrDefault();
            }
            return null;
        }
    }

    protected Guid? CountryId => HttpContext.Items["CountryId"] as Guid?;

    public Guid? CurrentUserId
    {
        get
        {
            // Check if the user is authenticated
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                // Try to get the user ID claim (e.g., "sub" in JWT tokens or "NameIdentifier" in Identity)
                var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier); // Or use a custom claim type like "userId"

                if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId))
                {
                    return userId;
                }
            }

            return null; // Return null if user is not authenticated or the claim is not found
        }
    }
}
