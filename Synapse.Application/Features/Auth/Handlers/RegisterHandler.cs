using MediatR;
using Synapse.Application.Common.Provider;
using Synapse.Application.Entities.Interfaces;
using Synapse.Application.Features.Auth.Commands;
using Synapse.Application.Features.Auth.Contracts;
using Synapse.Application.Features.Auth.Interfaces;
using Synapse.Domain.Entities;

namespace Synapse.Application.Features.Auth.Handlers;

public class RegisterHandler(
    IUserRepository userRepository,
    IPasswordProvider passwordProvider,
    IJwtProvider jwtProvider) : IRequestHandler<RegisterCommand, RegisterResponseContract>
{
    public async Task<RegisterResponseContract> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = new User(request.Username, passwordProvider.HashPassword(request.Password));
        var session = new UserSession(user.Id);

        user.AddSession(session);

        await userRepository.AddAsync(user);

        var (accessToken, refreshToken) = JwtHelper.GenerateTokens(session, jwtProvider);

        session.UpdateRefreshToken(refreshToken);

        await userRepository.SaveChangesAsync();

        return new RegisterResponseContract(accessToken);
    }
}