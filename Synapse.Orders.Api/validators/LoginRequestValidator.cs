using FluentValidation;
using Synapse.Orders.Api.DTOs;

namespace Synapse.Orders.Api.validators;

public class LoginRequestValidator: AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(item => item.username)
            .NotEmpty()
            .MinimumLength(4)
            .WithMessage("Username must be at least 4 characters long");
        
        RuleFor(item => item.password)
            .NotEmpty()
            .MinimumLength(6)
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,}$")
            .WithMessage("Password must be at least 6 characters long and contain at least 1 lowercase, 1 uppercase, 1 number and 1 special character.");
            
    }
}