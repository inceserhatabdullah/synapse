namespace Synapse.Infrastructure.Services;

public interface IPasswordService
{
    string HashPassword(string password);
    bool VerifyHashedPassword(string password, string hashedPassword);
}