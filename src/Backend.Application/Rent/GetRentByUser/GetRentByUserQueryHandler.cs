using Backend.Domain.DomainServices.RentStrategy;
using Backend.Domain.Repositories;
using Backend.Shared.CQRS.Queries;

namespace Backend.Application.Rent.GetRentByUser;

public class GetRentByUserQueryHandler(IRentRepository rentRepository, IRentCalculationStrategyFactory rentCalculationStrategyFactory) : QueryHandler<GetRentByUserQuery, GetRentByUserQueryResponse>
{
    public  override async Task<QueryResponse<GetRentByUserQueryResponse>> Handle(GetRentByUserQuery request, CancellationToken cancellationToken)
    {
        var rent = await rentRepository.GetRentByDriverId(request.DriverId);

        if (rent is null)
        {
            return default!;
        }

        var calcStrategy = rentCalculationStrategyFactory.GetStrategy(rent, rent.ExpectedEndDate);
        var total = calcStrategy.CalculateTotal(rent, rent.ExpectedEndDate);

        return new QueryResponse<GetRentByUserQueryResponse>(new GetRentByUserQueryResponse
        {
            PricePerDay = rent.Plan.PricePerDay,
            Total = total,
            PlanDays = rent.Plan.Days,
            RentId = rent.Id,
            StartDate = rent.StartDate,
            ExpectedEndDate = rent.ExpectedEndDate
        });
    }
}