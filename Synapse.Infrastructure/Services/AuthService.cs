namespace Synapse.Infrastructure.Services;

using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class AuthService : IAuthService
{
    public string GenerateToken(string username, string userId)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(DotNetEnv.Env.GetString("JWT_KEY")));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim("userId", userId),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var audiencesString = DotNetEnv.Env.GetString("JWT_AUDIENCES");
        var audiences = audiencesString.Split(',', StringSplitOptions.RemoveEmptyEntries);


        foreach (var audience in audiences)
        {
            claims.Add(new Claim(JwtRegisteredClaimNames.Aud, audience.Trim()));
        }


        var token = new JwtSecurityToken(
            issuer: DotNetEnv.Env.GetString("JWT_ISSUER"),
            claims: claims,
            audience: null,
            expires: DateTime.UtcNow.AddMinutes(DotNetEnv.Env.GetInt("JWT_EXPIRE_MINUTES", 60)),
            signingCredentials: cred
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}