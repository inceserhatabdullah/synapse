namespace Synapse.Infrastructure.Services;

public interface IAuthService
{
    string GenerateToken(string username, string userId);
}