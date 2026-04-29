using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyBudgetApp.API.Models;
using MyBudgetApp.API.Options;

namespace MyBudgetApp.API.Services.Access;

public class AccessTokenService
{
    private readonly JwtAccessOptions _options;

    public AccessTokenService(IOptions<JwtAccessOptions> options)
        => _options = options.Value;

    public string GenerateAccessToken(User user)
    {
        var claims = new[]
        {
            new Claim(
                ClaimTypes.Email,
                user.Email
                    ?? throw new InvalidOperationException("User email is null")
            )
        };
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_options.SigningKey)
        );
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_options.AccessMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
