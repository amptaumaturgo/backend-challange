using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Backend.Shared.CQRS.Commands;

public abstract class Command : IValidatableCommand, IRequest<CommandResponse>
{
    public Guid TrackerId { get; private set; } = Guid.NewGuid();
    public DateTime RequestedAt { get; private set; } = DateTime.Now;


    public ValidationResult Validate<TValidator, TInstance>(TValidator validator, TInstance instance)
        where TValidator : AbstractValidator<TInstance> where TInstance : class
    {
        var response = validator.Validate(instance);
        return response;
    }
}

public interface IValidatableCommand
{
    ValidationResult Validate<TValidator, TInstance>(TValidator validator, TInstance instance) where TValidator : AbstractValidator<TInstance> where TInstance : class;
}