using AutoMapper;
using Solidaridad.Application.Models.Constituency;
using Solidaridad.Core.Entities;

namespace Solidaridad.Application.MappingProfiles;

public class ConstituencyProfile : Profile
{
    public ConstituencyProfile()
    {
        CreateMap<Constituency, ConstituencyResponseModel>();
    }
}
