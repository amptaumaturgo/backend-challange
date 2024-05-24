using FluentValidation;

namespace Backend.Application.Motorcycle.Commands.RemoveMotorcycle;

public class RemoveMotorcycleCommandValidator : AbstractValidator<RemoveMotorcycleCommand>
{
    public RemoveMotorcycleCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .NotEmpty()
            .NotEqual(Guid.Empty);
    }
}