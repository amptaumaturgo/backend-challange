using AutoMapper;
using Backend.Domain.Repositories.Motorcycle;
using Backend.Shared.CQRS.Base;
using Backend.Shared.CQRS.Queries;

namespace Backend.Application.Motorcycle.Queries.GetAll;

public class GetAllQuery : Query<IEnumerable<GetAllQueryResponse>> { }

public class GetAllQueryHandler(IMotorcycleRepository motorcycleRepository, IMapper mapper) : QueryHandler<GetAllQuery, IEnumerable<GetAllQueryResponse>>
{
    public override async Task<QueryResponse<IEnumerable<GetAllQueryResponse>>> Handle(GetAllQuery request, CancellationToken cancellationToken)
    {
        var motorcycles = await motorcycleRepository.GetAllAsync();

        return mapper.Map<IEnumerable<GetAllQueryResponse>>(motorcycles).SuccessQueryResponse();
    }
}