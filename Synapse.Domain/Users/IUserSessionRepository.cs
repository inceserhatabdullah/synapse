namespace Synapse.Domain.Users;

public interface IUserSessionRepository
{
    Task<UserSession?> GetByIdAsync(Guid userId);
    Task AddUserSessionAsync(UserSession userSession);
    Task SaveChangesAsync();
}