using AutoMapper;
using Solidaridad.Application.Models.Farmer;
using Solidaridad.Application.Models.Ward;
using Solidaridad.Core.Entities;

namespace Solidaridad.Application.MappingProfiles;

public class WardProfile : Profile
{
    public WardProfile()
    {
        CreateMap<AdminLevel3, AdminLevel3ResponseModel>();
        
        CreateMap<CreateAdminLevel3Model, AdminLevel3>();
        
        CreateMap<AdminLevel3ResponseModel, AdminLevel3>();

        CreateMap<CreateModuleModel, AdminLevel3>();

        CreateMap<UpdateAdminLevel3Model, AdminLevel3>();
    }
}
