using AutoMapper;
using Solidaridad.Application.Models.ApplicationStatus;
using Solidaridad.Core.Entities.Loans;

namespace Solidaridad.Application.MappingProfiles;

public class ApplicationStatusProfile : Profile
{
    public ApplicationStatusProfile()
    {
        CreateMap<ApplicationStatus, ApplicationStatusResponseModel>();

        CreateMap<CreateApplicationStatusModel, ApplicationStatus>();

        CreateMap<ApplicationStatusResponseModel, ApplicationStatus>();

        CreateMap<UpdateApplicationStatusModel, ApplicationStatus>();
    }
}
