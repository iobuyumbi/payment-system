using AutoMapper;
using Solidaridad.Application.Models.DocumentType;
using Solidaridad.Core.Entities;

namespace Solidaridad.Application.MappingProfiles;

internal class DocumentTypeProfile : Profile
{
    public DocumentTypeProfile()
    {
        CreateMap<DocumentType, DocumentTypeResponseModel>();

        CreateMap<DocumentTypeResponseModel,  DocumentType> ();
    }
}
