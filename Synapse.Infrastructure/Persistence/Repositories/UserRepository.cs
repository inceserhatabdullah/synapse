using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Synapse.Application.Entities.Interfaces;
using Synapse.Domain.Entities;

namespace Synapse.Infrastructure.Persistence.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task<User?> GetAsync(Expression<Func<User, bool>> predicate)
    {
        return await context.Users.FirstOrDefaultAsync(predicate);
    }
    

    public async Task AddAsync(User user) => await context.Users.AddAsync(user);

    public async Task SaveChangesAsync() => await context.SaveChangesAsync();
}