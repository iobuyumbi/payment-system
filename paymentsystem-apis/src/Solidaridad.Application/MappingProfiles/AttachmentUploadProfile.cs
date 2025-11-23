using AutoMapper;
using Solidaridad.Application.Models.AttachmentUpload;
using Solidaridad.Core.Entities;

namespace Solidaridad.Application.MappingProfiles;

public class AttachmentUploadProfile : Profile
{
    public AttachmentUploadProfile()
    {
        CreateMap<AttachmentFile, AttachmentResponseModel>();

        CreateMap<CreateAttachmentUploadModel, AttachmentFile>();

        CreateMap<AttachmentResponseModel, AttachmentFile>();

        CreateMap<UpdateAttachmentUploadModel, AttachmentFile>();
    }
}
