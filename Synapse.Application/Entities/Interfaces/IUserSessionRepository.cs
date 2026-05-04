using System.Linq.Expressions;
using Synapse.Domain.Entities;

namespace Synapse.Application.Entities.Interfaces;

public interface IUserSessionRepository
{
    Task<UserSession?> GetAsync(Expression<Func<UserSession, bool>> predicate, params Expression<Func<UserSession, object>>[] includeProperties);
    Task AddAsync(UserSession session);
    Task SaveChangesAsync();
}