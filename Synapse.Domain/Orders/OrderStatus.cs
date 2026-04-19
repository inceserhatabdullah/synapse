namespace Synapse.Domain.Orders;

public enum OrderStatus
{
    Pending = 1,
    StockChecking = 2,
    AwaitingAI = 3,
    Comfirmed = 4,
    Cancelled = 5
}
