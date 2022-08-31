namespace MarketPlacement.Infrastructure.Authentication;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Entities.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtSettings _jwtSettings;

    public JwtTokenGenerator(IOptions<JwtSettings> jwtOptions)
    {
        
        _jwtSettings = jwtOptions.Value;
    }

    public string GenerateToken(User user, string role)
    {
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
            SecurityAlgorithms.HmacSha256);
        
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimsIdentity.DefaultRoleClaimType, role),
            new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
            new Claim(JwtRegisteredClaimNames.FamilyName, user.Lastname),
            
        };

        var securityToken = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            claims: claims, 
            signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }
}