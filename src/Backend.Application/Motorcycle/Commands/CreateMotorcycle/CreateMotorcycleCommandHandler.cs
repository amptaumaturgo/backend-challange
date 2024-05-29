using Backend.Application.Motorcycle.Events;
using Backend.Domain.Repositories.Motorcycle;
using Backend.Shared.CQRS.Base;
using Backend.Shared.CQRS.Commands;
using MediatR;

namespace Backend.Application.Motorcycle.Commands.CreateMotorcycle;

public class CreateMotorcycleCommandHandler(  IMotorcycleRepository motorcycleRepository, IMediator mediator) : CommandHandler<CreateMotorcycleCommand>
{
    public override async Task<CommandResponse> Handle(CreateMotorcycleCommand request, CancellationToken cancellationToken)
    {
        var validationResult = request.Validate(new CreateMotorcycleCommandValidator(), request);

        if (!validationResult.IsValid)
            return validationResult.FailResponse();

        var motorcycleExists = await motorcycleRepository.GetByPlate(request.Plate);
        if (motorcycleExists != null)
        {
            return "Motorcycle existent for the plate informed.".FailResponse();
        }

        await mediator.Publish(new CreateMotorcycleEvent(request.Year, request.Plate, request.Model), cancellationToken);
              
        return "Motorcycle sent to process.".SuccessResponse();
    }
}