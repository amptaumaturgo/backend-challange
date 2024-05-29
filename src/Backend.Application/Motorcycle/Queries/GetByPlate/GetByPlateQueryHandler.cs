using AutoMapper;
using Backend.Domain.Repositories.Motorcycle;
using Backend.Shared.CQRS.Base;
using Backend.Shared.CQRS.Queries;

namespace Backend.Application.Motorcycle.Queries.GetByPlate;

public class GetByPlateQueryHandler(IMotorcycleRepository motorcycleRepository, IMapper mapper) : QueryHandler<GetByPlateQuery, GetByPlateQueryResponse>
{
    public override async Task<QueryResponse<GetByPlateQueryResponse>> Handle(GetByPlateQuery request, CancellationToken cancellationToken)
    {
        var motorcycle = await motorcycleRepository.GetByPlate(request.Plate);

        return mapper.Map<GetByPlateQueryResponse>(motorcycle).SuccessQueryResponse();
    }
}