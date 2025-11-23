using AutoMapper;
using Solidaridad.Application.Models.ApplicationStatusLog;
using Solidaridad.Core.Entities.Loans;

namespace Solidaridad.Application.MappingProfiles;

public class ApplicationStatusLogProfile : Profile
{
    public ApplicationStatusLogProfile()
    {
        CreateMap<ApplicationStatusLog, ApplicationStatusLogResponseModel>();

        CreateMap<ApplicationStatusLogResponseModel, ApplicationStatusLog>();
    }
}
