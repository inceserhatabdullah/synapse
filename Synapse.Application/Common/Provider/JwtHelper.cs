using Synapse.Application.Features.Auth.Enums;
using Synapse.Application.Features.Auth.Interfaces;
using Synapse.Domain.Entities;

namespace Synapse.Application.Common.Provider;

public static class JwtHelper
{
    public static (string AccessToken, string RefreshToken) GenerateTokens(UserSession session, IJwtProvider jwtProvider) =>
    (
        jwtProvider.Generate(session, JwtEnum.AccessToken),
        jwtProvider.Generate(session, JwtEnum.RefreshToken)
    );
}