using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Solidaridad.API.Controllers;
using Solidaridad.Application.Services;
using System.IdentityModel.Tokens.Jwt;
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
            tokenHandler.ValidateToken(tokenRequest.api_token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _configuration["JwtConfiguration:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["JwtConfiguration:Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value;
            var username = jwtToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Name)?.Value ?? userId;

            if (!string.IsNullOrEmpty(userId))
            {
                // Get user permissions
                var permissions = await _accountService.GetPermissionsAsync(username, null);
                
                return Ok(new
                {
                    userId,
                    api_token = tokenRequest.api_token,
                    permissions = permissions?.Distinct().ToList(),
                    countries = new[] { new { code = 'KE', name = 'Kenya', id = 'default' } }
                });
            }
            return BadRequest(new { message = "Invalid token: User ID not found" });
        }
        catch (Exception ex)
        {
            return Unauthorized(new { message = "Token is invalid or expired", error = ex.Message });
        }
    } 
    #endregion
}

public class TokenRequest
{
    public string api_token { get; set; }
}
