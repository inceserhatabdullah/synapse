using Synapse.Application.Features.Auth.Interfaces;

namespace Synapse.Infrastructure.Auth;

public class PasswordProvider: IPasswordProvider
{
    public string HashPassword(string password) => BCrypt.Net.BCrypt.EnhancedHashPassword(password, workFactor: 11);

    public bool VerifyHashedPassword(string password, string hashedPassword) =>
        BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);
}