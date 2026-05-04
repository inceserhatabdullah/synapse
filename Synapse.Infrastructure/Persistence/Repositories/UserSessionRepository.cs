using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Synapse.Application.Entities.Interfaces;
using Synapse.Domain.Entities;

namespace Synapse.Infrastructure.Persistence.Repositories;

public class UserSessionRepository(AppDbContext context) : IUserSessionRepository
{
    public async Task<UserSession?> GetAsync(Expression<Func<UserSession, bool>> predicate,  params Expression<Func<UserSession, object>>[] includeProperties)
    {
        IQueryable<UserSession> query = context.UserSessions;

        if (includeProperties is not null)
        {
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
        }
        
        
        return await query.FirstOrDefaultAsync(predicate);
    }
    
    public async Task AddAsync(UserSession session) => await context.UserSessions.AddAsync(session);
    
    public async Task SaveChangesAsync() => await context.SaveChangesAsync();
}