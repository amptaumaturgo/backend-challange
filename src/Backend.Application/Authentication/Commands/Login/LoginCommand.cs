using Backend.Shared.CQRS.Commands;
using FluentValidation;

namespace Backend.Application.Authentication.Commands.Login;

public class LoginCommand : Command
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty();
    }
}