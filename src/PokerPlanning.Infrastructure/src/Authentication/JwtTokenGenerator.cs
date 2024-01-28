using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PokerPlanning.Application.src.Common.Interfaces.Authentication;
using PokerPlanning.Application.src.Common.Interfaces.Services;

namespace PokerPlanning.Infrastructure.src.Authentication;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly JwtSettings _jwtSettings;

    public JwtTokenGenerator(
        IDateTimeProvider dateTimeProvider,
        IOptions<JwtSettings> jwtOptions)
    {
        _dateTimeProvider = dateTimeProvider;
        _jwtSettings = jwtOptions.Value;
    }

    public string GenerateToken(string displayName, Guid userId, string role)
    {
        var claims = new List<Claim>
        {
            new (ClaimTypes.Name, displayName),
            new (ClaimTypes.NameIdentifier, userId.ToString()),
            new (ClaimTypes.Role, role),
        };

        return GenerateToken(claims);
    }

    private string GenerateToken(List<Claim> claims)
    {
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
                SecurityAlgorithms.HmacSha256);


        var securityToken = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            expires: _dateTimeProvider.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            claims: claims,
            audience: _jwtSettings.Audience,
            signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }
}