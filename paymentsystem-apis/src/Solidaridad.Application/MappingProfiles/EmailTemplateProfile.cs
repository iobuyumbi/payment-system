using AutoMapper;
using Solidaridad.Application.Models.Email;
using Solidaridad.Core.Entities.Email;

namespace Solidaridad.Application.MappingProfiles;

public class EmailTemplateProfile : Profile
{
    public EmailTemplateProfile()
    {
        CreateMap<CreateEmailTemplateModel, EmailTemplate>()
            .ForMember(ti => ti.IsDeleted, ti => ti.MapFrom(cti => false));

        CreateMap<UpdateEmailTemplateModel, EmailTemplate>();

        CreateMap<EmailTemplate, EmailTemplateResponseModel>();

        CreateMap<EmailTemplateResponseModel, EmailTemplate>();

        CreateMap<CreateEmailTemplateVariableModel, EmailTemplateVariable>();

        CreateMap<EmailTemplateVariable, EmailTemplateVariableResponseModel>();
    }
}
