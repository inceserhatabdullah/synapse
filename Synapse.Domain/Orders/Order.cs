namespace Synapse.Domain.Orders;

public class Order
{
    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public OrderStatus Status { get; private set; }

    private readonly List<OrderItem> _items = new();
    public IReadOnlyCollection<OrderItem> Items => _items;
    
    private Order() {}

    public Order(Guid customerId)
    {
        Id = Guid.NewGuid();
        CustomerId = customerId;
        Status = OrderStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }
   
    public void AddItem(Guid productId, int quantity, decimal price)
    {
        var item = new OrderItem(Id, productId, quantity, price);
        _items.Add(item);
    }
}
