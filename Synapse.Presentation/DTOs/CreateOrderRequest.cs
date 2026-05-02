namespace Synapse.Presentation.DTOs;

public record CreateOrderRequest(List<OrderItemRequest> Items);

public record OrderItemRequest(Guid ProductId, int Quantity, decimal Price);