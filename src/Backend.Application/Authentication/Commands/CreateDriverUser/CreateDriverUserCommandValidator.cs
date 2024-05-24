using FluentValidation;

namespace Backend.Application.Authentication.Commands.CreateDriverUser;

public class CreateDriverUserCommandValidator : AbstractValidator<CreateDriverUserCommand>
{
    public CreateDriverUserCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name cannot be longer than 100 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

        RuleFor(x => x.Cnpj)
            .NotEmpty().WithMessage("CNPJ is required.")
            .Length(14).WithMessage("CNPJ must be 14 characters long.");

        RuleFor(x => x.BirthDate)
            .NotEmpty().WithMessage("Birth date is required.")
            .Must(date => date <= DateTime.UtcNow.Date).WithMessage("Birth date cannot be in the future.");

        RuleFor(x => x.DriverLicenseNumber)
            .NotEmpty().WithMessage("Driver's license number is required.")
            .Length(7).WithMessage("Driver's license number must be 7 characters long.");

        RuleFor(x => x.DriverLicenseType)
            .NotEmpty().WithMessage("Driver's license type is required.");

    }

}