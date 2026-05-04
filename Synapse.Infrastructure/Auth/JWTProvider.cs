using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Synapse.Application.Features.Auth.Enums;
using Synapse.Application.Features.Auth.Interfaces;
using Synapse.Application.Features.Auth.Records;
using Synapse.Domain.Entities;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Synapse.Infrastructure.Auth;

public class JWTProvider(IConfiguration configuration) : IJwtProvider
{
    private readonly IReadOnlyDictionary<JwtEnum, JwtRecord> _records = new Dictionary<JwtEnum, JwtRecord>
    {
        {
            JwtEnum.AccessToken,
            new JwtRecord(
                Secret: configuration["JWT_ACCESS_SECRET_KEY"], ExpiresIn: configuration["JWT_ACCESS_EXPIRE_MINUTES"]
            )
        },
        {
            JwtEnum.RefreshToken,
            new JwtRecord(
                Secret: configuration["JWT_REFRESH_SECRET_KEY"], ExpiresIn: configuration["JWT_REFRESH_EXPIRE_DAYS"]
            )
        }
    };

    public string Generate(UserSession session, JwtEnum type)
    {
        var config = _records[type];

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Secret));
        var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, session.User.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Sid, session.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Name, session.User.Username),
        };

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(ParseDuration(config.ExpiresIn)),
            SigningCredentials = credential,
        };

        var handler = new JwtSecurityTokenHandler();
        var token = handler.CreateToken(descriptor);

        return handler.WriteToken(token);
    }

    public ClaimsPrincipal? Verify(string token, JwtEnum type)
    {
        var configuration = _records[type];
        var handler = new JwtSecurityTokenHandler();

        try
        {
            return handler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.Secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero,
            }, out _);
        }
        catch
        {
            return null;
        }
    }

    private TimeSpan ParseDuration(string duration)
    {
        if (int.TryParse(duration, out var minutes))
            return TimeSpan.FromMinutes(minutes);

        var value = int.Parse(duration[..^1]);
        return duration.EndsWith("d") ? TimeSpan.FromDays(value) : TimeSpan.FromHours(value);
    }
}