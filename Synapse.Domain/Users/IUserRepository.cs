namespace Synapse.Domain.Users;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid userId);
    Task AddUserAsync(User user);
    Task SaveChangesAsync();
}