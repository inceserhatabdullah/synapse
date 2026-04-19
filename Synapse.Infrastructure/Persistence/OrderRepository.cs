using Microsoft.EntityFrameworkCore;
using Synapse.Domain.Orders;

namespace Synapse.Infrastructure.Persistence;

public class OrderRepository(AppDbContext context): IOrderRepository
{
    public async Task<Order?> GetByIdAsync(Guid id)
    {
        return await context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id);
    }
    
    public async Task AddAsync(Order order)
    {
        await context.Orders.AddAsync(order);
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}