using Backend.Domain.Repositories.Motorcycle;
using Backend.Domain.Repositories.Rent;
using Backend.Shared.CQRS.Base;
using Backend.Shared.CQRS.Commands;
using Backend.Shared.Data;

namespace Backend.Application.Motorcycle.Commands.RemoveMotorcycle;

public class RemoveMotorcycleCommandHandler(IMotorcycleRepository motorcycleRepository, IRentRepository rentRepository, IUnitOfWork unitOfWork) : CommandHandler<RemoveMotorcycleCommand>
{
    public override async Task<CommandResponse> Handle(RemoveMotorcycleCommand request, CancellationToken cancellationToken)
    {
        var validationResult = request.Validate(new RemoveMotorcycleCommandValidator(), request);

        if (!validationResult.IsValid)
            return validationResult.FailResponse();

        var motorcycle = await motorcycleRepository.GetByIdAsync(request.Id);
        if (motorcycle is null)
        {
            return "Motorcycle not found.".FailResponse();
        }

        var hasRentToMotorcycle = await rentRepository.HasRentToMotorcycle(request.Id);
        if(hasRentToMotorcycle)
        {
            return "Has rent to motorcycle.".FailResponse();
        }

        motorcycleRepository.Remove(motorcycle);

        await unitOfWork.CommitAsync();

        return "Success to remove motorcycle.".SuccessResponse();
    }
}