using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Solidaridad.API.Controllers;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

[AllowAnonymous]
public class TokenController : ApiController
{
    #region DI
    private readonly IConfiguration _configuration;

    public TokenController(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    #endregion

    #region Methods
    [AllowAnonymous]
    [HttpPost("verify_token")]
    public IActionResult VerifyToken(TokenRequest tokenRequest)
    {
        if (string.IsNullOrEmpty(tokenRequest.api_token))
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

            if (!string.IsNullOrEmpty(userId))
            {
                return Ok(new
                {
                    userId,
                    api_token = tokenRequest.api_token
                });
            }
            return null;
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
