using FluentValidation;

namespace Backend.Application.Motorcycle.Commands.CreateMotorcycle;

public class CreateMotorcycleCommandValidator : AbstractValidator<CreateMotorcycleCommand>
{
    public CreateMotorcycleCommandValidator()
    {
        RuleFor(x => x.Year)
            .NotEmpty()
            .WithMessage("Year is required")
            .GreaterThan(2010)
            .WithMessage("Year must be greater than 2010");

        RuleFor(x => x.Model)
            .NotEmpty()
            .WithMessage("Model is required") ;

        RuleFor(x => x.Plate)
            .NotEmpty()
            .WithMessage("Plate is required");
    }

}