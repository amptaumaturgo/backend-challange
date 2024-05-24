using AutoMapper;
using Backend.Domain.Entities;

namespace Backend.Application.Rent.GetAvailablePlans;

public class GetAvailablePlansQueryProfile : Profile
{
    public GetAvailablePlansQueryProfile()
    {
        CreateMap<Plan, GetAvailablePlansQueryResponse>()
            .ForMember(x => x.PricePerDay, x => x.MapFrom(x => $"R${x.PricePerDay}"));
    }
}