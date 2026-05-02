using FluentValidation;
using Synapse.Presentation.DTOs;

namespace Synapse.Presentation.validators;

public class CreateOrderRequestValidator: AbstractValidator<CreateOrderRequest>
{
    public CreateOrderRequestValidator()
    {
        RuleFor(item => item.Items).NotEmpty().WithMessage("Order must have at least one item");

        RuleForEach(item => item.Items).SetValidator(new OrderItemRequestValidator());
    }
}

public class OrderItemRequestValidator : AbstractValidator<OrderItemRequest>
{
    public OrderItemRequestValidator()
    {
        RuleFor(item => item.ProductId).NotEmpty().WithMessage("Order must have at least one product");
        RuleFor(item => item.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than zero");
        RuleFor(item => item.Price).GreaterThan(0).WithMessage("Price must be greater than zero");
    }
}