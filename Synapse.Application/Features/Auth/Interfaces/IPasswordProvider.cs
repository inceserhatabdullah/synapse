namespace Synapse.Application.Features.Auth.Interfaces;

public interface IPasswordProvider
{
    string HashPassword(string password);
    bool VerifyHashedPassword(string password, string hashedPassword);
}