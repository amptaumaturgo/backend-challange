using Backend.Application.Rent.RentMotorcycle.RentMotorcycleSpecification;
using Backend.Domain.DomainServices.RentStrategy;
using Backend.Domain.Repositories;
using Backend.Shared.CQRS.Base;
using Backend.Shared.CQRS.Commands;
using Backend.Shared.Data;

namespace Backend.Application.Rent.RentMotorcycle;

public class RentMotorcycleCommandHandler(
    ISpecification<Domain.Entities.Driver> driverSpecification,
    ISpecification<Domain.Entities.Motorcycle> motorcycleSpecification,
    ISpecification<Domain.Entities.Rent> rentSpecification,
    IRentRepository rentRepository,
    IRentCalculationStrategyFactory rentCalculationStrategyFactory,
    IUnitOfWork unitOfWork)
    : CommandHandler<RentMotorcycleCommand>
{
    public override async Task<CommandResponse> Handle(RentMotorcycleCommand request,
        CancellationToken cancellationToken)
    {
        List<string> specificationErrors = new();

        await driverSpecification.IsSatisfiedBy(request.DriverId, specificationErrors);
        await motorcycleSpecification.IsSatisfiedBy(request.MotorcycleId, specificationErrors);
        await rentSpecification.IsSatisfiedBy(request.PlanId, specificationErrors);
        
        if (specificationErrors.Any())
        {
            return specificationErrors.FailResponse();
        }

        var plan = await rentRepository.GetPlanById(request.PlanId);

        var rent = new Domain.Entities.Rent(request.DriverId, request.MotorcycleId, plan!);

        rentRepository.Add(rent);

        await unitOfWork.CommitAsync();

        var calcStrategy = rentCalculationStrategyFactory.GetStrategy(rent, request.ExpectedDevolutionDate);
        var total = calcStrategy.CalculateTotal(rent, request.ExpectedDevolutionDate);

        return new RentMotorcycleCommandResponse(total, plan!.PricePerDay, plan.Days).SuccessResponse();
    }
}