using FluentValidation;
using Synapse.Application.Features.Auth.Contracts;

namespace Synapse.Application.Features.Auth.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequestContract>
{
    public LoginRequestValidator()
    {
        RuleFor(request => request.username)
            .NotEmpty()
            .WithMessage("Username cannot be empty.");

        RuleFor(request => request.password)
            .NotEmpty()
            .WithMessage("Password cannot be empty.");
    }
}