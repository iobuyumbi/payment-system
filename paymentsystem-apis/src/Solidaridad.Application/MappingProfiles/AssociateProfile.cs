using AutoMapper;
using Solidaridad.Application.Models.Associate;
using Solidaridad.Core.Entities.Payments;

namespace Solidaridad.Application.MappingProfiles;

public class AssociateProfile : Profile
{
    public AssociateProfile()
    {
        CreateMap<AssociateMap,AssociateResponseModel>();

        CreateMap<CreateAssociateModel, AssociateMap>();

        CreateMap<AssociateResponseModel, AssociateMap>();

        CreateMap<UpdateAssociateModel, AssociateMap>();
    }

}
