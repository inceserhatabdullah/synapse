using Microsoft.EntityFrameworkCore;
using Synapse.Domain.Orders;
using Synapse.Domain.Entities;

namespace Synapse.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserSession> UserSessions { get; set; }
    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            var property = entry.Entity.GetType().GetProperty("UpdatedAt");
            if (property != null)
            {
                property.SetValue(entry.Entity, DateTime.UtcNow);
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}