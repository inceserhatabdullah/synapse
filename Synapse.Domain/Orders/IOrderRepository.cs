using Synapse.Domain.Orders;

namespace Synapse.Domain.Orders;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(Guid id);
    Task AddAsync(Order order);
    Task SaveChangesAsync();

}