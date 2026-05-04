using System.Security.Claims;
using Synapse.Application.Features.Auth.Enums;
using Synapse.Domain.Entities;

namespace Synapse.Application.Features.Auth.Interfaces;

public interface IJwtProvider
{
    string Generate(UserSession session, JwtEnum type);
    ClaimsPrincipal? Verify(string token, JwtEnum type);
}