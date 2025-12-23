using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Solidaridad.API.Controllers;
using Solidaridad.Application.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[AllowAnonymous]
public class TokenController : ApiController
{
    #region DI
    private readonly IConfiguration _configuration;
    private readonly IAccountService _accountService;

    public TokenController(IConfiguration configuration, IAccountService accountService)
    {
        _configuration = configuration;
        _accountService = accountService;
    }
    #endregion

    #region Methods
    [AllowAnonymous]
    [HttpPost("verify_token")]
    public async Task<IActionResult> VerifyToken([FromBody] TokenRequest tokenRequest)
    {
        if (tokenRequest == null || string.IsNullOrEmpty(tokenRequest.api_token))
        {
            return BadRequest(new { message = "Token is required" });
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["JwtConfiguration:SecretKey"]);

        try
        {
            // Create validation parameters that match the authentication middleware
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,  // Must match the authentication middleware
                ValidateAudience = false, // Must match the authentication middleware
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            // Manually validate the token
            var principal = tokenHandler.ValidateToken(tokenRequest.api_token, validationParameters, out var validatedToken);

            var username = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                           ?? principal.FindFirst(ClaimTypes.Name)?.Value
                           ?? principal.Identity?.Name;

            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value
                         ?? principal.FindFirst(JwtRegisteredClaimNames.NameId)?.Value;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(userId))
            {
                Console.WriteLine($"[TokenController] verify_token missing required claims");
                return Unauthorized(new { message = "Invalid token", error = "Missing required claims" });
            }

            // Get user permissions
            var permissions = await _accountService.GetPermissionsAsync(username, null);
            Console.WriteLine($"[TokenController] verify_token username={username} userId={userId} permissions={(permissions?.Count() ?? 0)}");

            return Ok(new
            {
                userId,
                username,
                api_token = tokenRequest.api_token,
                permissions = permissions?.Distinct().ToList() ?? new List<string>(),
                countries = new[] { new { 
                    code = "KE", 
                    name = "Kenya", 
                    id = "default",
                    currencyName = "Kenyan Shilling",
                    currencyPrefix = "KES",
                    currencySuffix = ""
                } }
            });
        }
        catch (SecurityTokenExpiredException)
        {
            return Unauthorized(new { message = "Token has expired" });
        }
        catch (SecurityTokenInvalidSignatureException ex)
        {
            Console.WriteLine($"[TokenController] Signature validation failed: {ex.Message}");
            return Unauthorized(new { message = "Token signature is invalid", error = ex.Message });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[TokenController] Token validation error: {ex.GetType().Name} - {ex.Message}");
            return Unauthorized(new { message = "Invalid token", error = ex.Message });
        }
    } 
    #endregion
}

public class TokenRequest
{
    public string api_token { get; set; }
}
