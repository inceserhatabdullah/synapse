using Synapse.Domain.Orders;

namespace Synapse.Orders.Api.endpoints;

public class OrderEndpoints
{
    public void MapEndpoints(IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/orders");
        group.MapPost("/", CreateOrder);
        group.MapGet("/{id:guid}", GetOrder);
    }

    private async Task<IResult> CreateOrder(IOrderRepository repository)
    {
        var order = new Order(Guid.NewGuid());
        order.AddItem(Guid.NewGuid(), 2, 150.00m);

        await repository.AddAsync(order);
        await repository.SaveChangesAsync();
        
        return Results.Created($"/orders/{order.Id}", order);
    }

    private async Task<IResult> GetOrder(Guid id, IOrderRepository repository)
    {
        var order = await repository.GetByIdAsync(id);
        return order is not null ? Results.Ok(order) : Results.NotFound();
    }
}