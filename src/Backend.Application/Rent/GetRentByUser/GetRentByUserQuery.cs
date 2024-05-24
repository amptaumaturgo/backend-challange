using AutoMapper;
using Backend.Shared.CQRS.Base;
using Backend.Shared.CQRS.Queries;

namespace Backend.Application.Rent.GetRentByUser;

public class GetRentByUserQuery : Query<GetRentByUserQueryResponse>
{
    public Guid DriverId { get; set; }
}