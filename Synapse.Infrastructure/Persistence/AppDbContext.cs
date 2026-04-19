using Microsoft.EntityFrameworkCore;
using Synapse.Domain.Orders;

namespace Synapse.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options): DbContext(options)
{
    
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(builder =>
        {
            var navigation = builder.Metadata.FindNavigation(nameof(Order.Items));
           navigation?.SetPropertyAccessMode(PropertyAccessMode.Field);
            
        });
    }
}