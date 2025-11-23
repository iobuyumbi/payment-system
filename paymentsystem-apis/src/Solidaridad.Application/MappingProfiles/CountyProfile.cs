using AutoMapper;
using Solidaridad.Application.Models.County;
using Solidaridad.Application.Models.Farmer;
using Solidaridad.Core.Entities;

namespace Solidaridad.Application.MappingProfiles;

public class CountyProfile : Profile
{
    public CountyProfile()
    {
        CreateMap<AdminLevel1, AdminLevel1ResponseModel>();
        
        CreateMap<CreateAdminLevel1Model, AdminLevel1>();
        
        CreateMap<AdminLevel1ResponseModel, AdminLevel1>();

        CreateMap<CreateModuleModel, AdminLevel1>();

        CreateMap<UpdateAdminLevel1Model, AdminLevel1>();
    }
}
