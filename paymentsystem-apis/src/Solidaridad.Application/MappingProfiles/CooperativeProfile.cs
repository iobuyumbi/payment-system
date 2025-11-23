using AutoMapper;
using Solidaridad.Application.Models.Cooperative;
using Solidaridad.Core.Entities;

namespace Solidaridad.Application.MappingProfiles;

public class CooperativeProfile : Profile
{
    public CooperativeProfile()
    {
        CreateMap<Cooperative, CooperativeResponseModel>();

        CreateMap<ImportCoperativeModel, Cooperative>();

        CreateMap<CreateCooperativeModel, Cooperative>();

        CreateMap<CooperativeResponseModel, Cooperative>();

        CreateMap<UpdateCooperativeModel, Cooperative>();
    }

}
