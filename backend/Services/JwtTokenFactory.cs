using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Backend.DTO.Users;
using Backend.Models;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Services;

public static class JwtTokenFactory
{
    public static LoginResponse CreateToken(User user, IConfiguration config)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY")!)
        );

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(type: JwtRegisteredClaimNames.Sub, value: user.ID.ToString()),
            new Claim(type: JwtRegisteredClaimNames.UniqueName, value: user.UserName),
            new Claim(type: ClaimTypes.NameIdentifier, value: user.ID.ToString()),
            new Claim(type: ClaimTypes.Name, value: user.UserName),
        };

        if (!string.IsNullOrWhiteSpace(user.Role))
            claims.Add(new Claim(type: ClaimTypes.Role, value: user.Role));

        var expires = DateTime.UtcNow.AddHours(2);

        var token = new JwtSecurityToken
        (
        issuer: Environment.GetEnvironmentVariable("JWT_ISSUER"),
        audience: Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
        claims: claims,
        expires: expires,
        signingCredentials: credentials
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return new LoginResponse(jwt, expires);
    }
}
