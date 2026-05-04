using MediatR;
using Synapse.Application.Common.Provider;
using Synapse.Application.Entities.Interfaces;
using Synapse.Application.Features.Auth.Commands;
using Synapse.Application.Features.Auth.Contracts;
using Synapse.Application.Features.Auth.Interfaces;
using Synapse.Domain.Entities;

namespace Synapse.Application.Features.Auth.Handlers;

public class LoginHandler(
    IUserRepository userRepository,
    IUserSessionRepository userSessionRepository,
    IPasswordProvider passwordProvider,
    IJwtProvider jwtProvider
) : IRequestHandler<LoginCommand, LoginResponseContract>
{
    public async Task<LoginResponseContract> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetAsync(u => u.Username == request.Username);

        if (user is null)
        {
            throw new Exception("Invalid credentials");
        }

        var isPasswordValid = passwordProvider.VerifyHashedPassword(request.Password, user.Password);

        if (!isPasswordValid)
        {
            throw new Exception("Invalid credentials");
        }

        var session = await userSessionRepository.GetAsync(s => s.UserId == user.Id, s => s.User);

        if (session is null)
        {
            session = new UserSession(user.Id);
            user.AddSession(session);

            await userSessionRepository.AddAsync(session);
        }

        var (accessToken, refreshToken) = JwtHelper.GenerateTokens(session, jwtProvider);

        session.UpdateRefreshToken(refreshToken);

        await userSessionRepository.SaveChangesAsync();

        return new LoginResponseContract(accessToken);
    }
}