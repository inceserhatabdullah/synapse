using System.Linq.Expressions;
using Synapse.Domain.Entities;

namespace Synapse.Application.Entities.Interfaces;

public interface IUserRepository
{
    Task<User?> GetAsync(Expression<Func<User, bool>> predicate);
    Task AddAsync(User user);
    Task SaveChangesAsync();
}