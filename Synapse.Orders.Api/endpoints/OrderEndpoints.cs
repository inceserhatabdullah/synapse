using FluentValidation;
using Synapse.Domain.Orders;
using Synapse.Orders.Api.DTOs;

namespace Synapse.Orders.Api.endpoints;

public class OrderEndpoints : IEndpointDefinition
{
    public void MapEndpoints(IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/orders").RequireAuthorization();
        group.MapPost("/", CreateOrder);
        group.MapGet("/{id:guid}", GetOrder);
    }

    private async Task<IResult> CreateOrder(
        CreateOrderRequest request,
        IOrderRepository repository,
        IValidator<CreateOrderRequest> validator)
    {
        var validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        // TODO: customerId jwt içerisinden gelecek.
        var currentUserId = Guid.NewGuid();
        var order = new Order(currentUserId);

        foreach (var item in request.Items)
        {
            order.AddItem(item.ProductId, item.Quantity, item.Price);
        }


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