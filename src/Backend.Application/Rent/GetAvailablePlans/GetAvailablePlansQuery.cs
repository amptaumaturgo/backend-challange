using AutoMapper;
using Backend.Domain.Repositories;
using Backend.Shared.CQRS.Base;
using Backend.Shared.CQRS.Queries;

namespace Backend.Application.Rent.GetAvailablePlans;

public class GetAvailablePlansQuery : Query<IEnumerable<GetAvailablePlansQueryResponse>> { }

public class GetAvailablePlansQueryHandler(IRentRepository rentRepository, IMapper mapper) : QueryHandler<GetAvailablePlansQuery, IEnumerable<GetAvailablePlansQueryResponse>>
{
    public override async Task<QueryResponse<IEnumerable<GetAvailablePlansQueryResponse>>> Handle(GetAvailablePlansQuery request, CancellationToken cancellationToken)
    {
        var plans = await rentRepository.GetPlans();

        var response = mapper.Map<IEnumerable<GetAvailablePlansQueryResponse>>(plans);

        return response.SuccessQueryResponse();
    }
}

public class GetAvailablePlansQueryResponse
{
    public Guid Id { get; set; }
    public string PricePerDay { get; set; } = string.Empty;
    public int Days { get; set; }
}