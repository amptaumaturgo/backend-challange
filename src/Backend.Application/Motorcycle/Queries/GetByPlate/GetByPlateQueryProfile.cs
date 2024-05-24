using AutoMapper;

namespace Backend.Application.Motorcycle.Queries.GetByPlate;

public class GetByPlateQueryProfile : Profile
{
    public GetByPlateQueryProfile()
    {
        CreateMap<Domain.Entities.Motorcycle, GetByPlateQueryResponse>();
    }
}