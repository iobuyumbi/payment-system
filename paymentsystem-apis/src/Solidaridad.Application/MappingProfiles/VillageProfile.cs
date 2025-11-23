using AutoMapper;
using Solidaridad.Application.Models.Farmer;
using Solidaridad.Application.Models.Village;
using Solidaridad.Core.Entities;

namespace Solidaridad.Application.MappingProfiles;

public class VillageProfile : Profile
{
    public VillageProfile() 
    {
        CreateMap<AdminLevel4, AdminLevel4ResponseModel>();
        
        CreateMap<CreateAdminLevel4Model, AdminLevel4>();
        
        CreateMap<AdminLevel4ResponseModel, AdminLevel4>();

        CreateMap<CreateModuleModel, AdminLevel4>();

        CreateMap<UpdateAdminLevel4Model, AdminLevel4>();
    }
}
