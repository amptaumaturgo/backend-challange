using Backend.Domain.Repositories.Motorcycle;
using Backend.Shared.CQRS.Base;
using Backend.Shared.CQRS.Commands;
using Backend.Shared.Data;

namespace Backend.Application.Motorcycle.Commands.UpdateMotorcyclePlate;

public class UpdateMotorcyclePlateCommandHandler(IMotorcycleRepository motorcycleRepository, IUnitOfWork unitOfWork) : CommandHandler<UpdateMotorcyclePlateCommand>
{
    public override async Task<CommandResponse> Handle(UpdateMotorcyclePlateCommand request, CancellationToken cancellationToken)
    {
        var motorcycle = await motorcycleRepository.GetByIdAsync(request.Id);
        if (motorcycle is null)
        {
            return "Motorcycle not found.".FailResponse();
        }

        motorcycle.SetPlate(request.Plate);

        motorcycleRepository.Update(motorcycle);

        await unitOfWork.CommitAsync();

        return "Success to save motorcycle.".SuccessResponse();
    }
}