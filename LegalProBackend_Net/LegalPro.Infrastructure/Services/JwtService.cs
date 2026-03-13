using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LegalPro.Application.Common.Interfaces;
using LegalPro.Domain.Entities;

namespace LegalPro.Infrastructure.Services;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(Usuario usuario)
    {
        // JWT_SECRET = env var Railway (prioridad) | JwtSettings:Secret = appsettings (fallback local)
        var secret = _configuration["JWT_SECRET"]
                     ?? _configuration["JwtSettings:Secret"]
                     ?? throw new InvalidOperationException("JWT_SECRET no está configurado.");
        var expiryDays = int.TryParse(_configuration["JwtSettings:ExpiryDays"], out var d) ? d : 7;

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
            new Claim(ClaimTypes.Role, usuario.Rol.ToString()),
            new Claim("Especialidad", usuario.Especialidad.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: "LegalProAPI",
            audience: "LegalProClients",
            claims: claims,
            expires: DateTime.UtcNow.AddDays(expiryDays),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
