using AutoMapper;

namespace Backend.Application.Motorcycle.Queries.GetAll;

public class GetAllQueryProfile : Profile
{
    public GetAllQueryProfile()
    {
        CreateMap<Domain.Entities.Motorcycle, GetAllQueryResponse>();
    }
}