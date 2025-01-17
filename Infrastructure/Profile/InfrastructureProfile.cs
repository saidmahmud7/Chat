using Domain.DTO_s;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Profile;

public class InfrastructureProfile : AutoMapper.Profile
{
    public InfrastructureProfile()
    {
        CreateMap<Car, GetCarDto>();
        CreateMap<CreateCarDto, Car>();
    }
}