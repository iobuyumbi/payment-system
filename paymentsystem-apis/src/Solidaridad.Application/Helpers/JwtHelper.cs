using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Solidaridad.DataAccess.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Solidaridad.Application.Helpers;

public static class JwtHelper
{
    public static string GenerateToken(ApplicationUser user, IConfiguration configuration, DateTime validTo)
    {
        var secretKey = configuration.GetValue<string>("JwtConfiguration:SecretKey");

        var key = Encoding.UTF8.GetBytes(secretKey);

        var tokenHandler = new JwtSecurityTokenHandler();

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            }),
            Issuer = configuration.GetValue<string>("JwtConfiguration:Issuer"),
            Audience = configuration.GetValue<string>("JwtConfiguration:Audience"),
            Expires = validTo,
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public static string GenerateRefreshToken(ApplicationUser user, IConfiguration configuration)
    {
        var secretKey = configuration.GetValue<string>("JwtConfiguration:SecretKey");

        var key = Encoding.UTF8.GetBytes(secretKey);

        var tokenHandler = new JwtSecurityTokenHandler();

        var refreshToken = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            }),
            Issuer = configuration.GetValue<string>("JwtConfiguration:Issuer"),
            Audience = configuration.GetValue<string>("JwtConfiguration:Audience"),
            Expires = DateTime.UtcNow.AddHours(4),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(refreshToken);

        return tokenHandler.WriteToken(token);
    }
}
