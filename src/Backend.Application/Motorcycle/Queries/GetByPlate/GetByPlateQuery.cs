using Backend.Shared.CQRS.Queries;

namespace Backend.Application.Motorcycle.Queries.GetByPlate;

public class GetByPlateQuery : Query<GetByPlateQueryResponse>
{
    public string Plate { get; set; } = string.Empty;
}