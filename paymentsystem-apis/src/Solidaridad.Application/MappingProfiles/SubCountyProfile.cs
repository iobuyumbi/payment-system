using AutoMapper;
using Solidaridad.Application.Models.Farmer;
using Solidaridad.Application.Models.SubCounty;
using Solidaridad.Core.Entities;

namespace Solidaridad.Application.MappingProfiles;

public class SubCountyProfile : Profile
{
    public SubCountyProfile()
    {
        CreateMap<AdminLevel2, AdminLevel2ResponseModel>();

        CreateMap<CreateAdminLevel2Model, AdminLevel2>();

        CreateMap<AdminLevel2ResponseModel, AdminLevel2>();

        CreateMap<CreateModuleModel, AdminLevel2>();

        CreateMap<UpdateAdminLevel2Model, AdminLevel2>();
    }
}
