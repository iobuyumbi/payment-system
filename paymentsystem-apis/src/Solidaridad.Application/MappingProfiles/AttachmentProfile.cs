using AutoMapper;
using Solidaridad.Application.Models.LoanAttachmentModel;
using Solidaridad.Core.Entities;

namespace Solidaridad.Application.MappingProfiles;

public class AttachmentProfile : Profile
{
    public AttachmentProfile()
    {
        CreateMap<AttachmentMapping, MappingResponseModel>();

        CreateMap<CreateAttachmentModel, AttachmentMapping>();

        CreateMap<MappingResponseModel, AttachmentMapping>();

        CreateMap<UpdateAttachmentModel, AttachmentMapping>();
    }
}
