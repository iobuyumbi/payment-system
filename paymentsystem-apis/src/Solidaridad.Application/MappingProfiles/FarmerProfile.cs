using AutoMapper;
using Solidaridad.Application.Models.Farmer;
using Solidaridad.Core.Entities;
using System.Security.AccessControl;

namespace Solidaridad.Application.MappingProfiles;

public class FarmerProfile : Profile
{
    public FarmerProfile()
    {
        CreateMap<CreateFarmerModel, Farmer>()
        .ForMember(ti => ti.DocumentType, ti => ti.Ignore())
        .ForMember(ti => ti.DocumentTypeId, ti => ti.Ignore());

        CreateMap<FarmerResponseModel, Farmer>();

        CreateMap<Farmer, FarmerResponseModel>()
    .ForPath(dest => dest.DocumentType.Value, opt => opt.MapFrom(src => src.DocumentType.Id));

        CreateMap<CreateModuleModel, Farmer>();

        CreateMap<ImportFarmerModel, Farmer>().ForMember(ti => ti.AdminLevel1, ti => ti.MapFrom(cti => string.Empty));

        CreateMap<UpdateFarmerModel, Farmer>()
          .ForMember(ti => ti.DocumentType, ti => ti.Ignore())
          .ForMember(ti => ti.DocumentTypeId, ti => ti.Ignore());;
    }

}
