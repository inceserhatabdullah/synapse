using FluentValidation;
using Synapse.Application.Features.Auth.Commands;
using Synapse.Application.Features.Auth.Contracts;

namespace Synapse.Application.Features.Auth.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterCommand>
{
    public RegisterRequestValidator()
    {
        RuleFor(request => request.Username)
            .NotEmpty()
            .MinimumLength(4)
            .WithMessage("Username must be at least 4 characters long");

        RuleFor(request => request.Password)
            .NotEmpty()
            .MinimumLength(6)
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,}$")
            .WithMessage(
                "Password must be at least 6 characters long and contain at least 1 lowercase, 1 uppercase, 1 number and 1 special character.");
    }
}