using AutoMapper;
using Solidaridad.Application.Models.ActivityLog;
using Solidaridad.Core.Entities;

namespace Solidaridad.Application.MappingProfiles;

public class ActivityLogProfile : Profile
{
    public ActivityLogProfile()
    {
        CreateMap<CreateActivityLogModel, ActivityLog>()
            .ForMember(ti => ti.IsDeleted, ti => ti.MapFrom(cti => false));

        CreateMap<UpdateActivityLogModel, ActivityLog>();

        CreateMap<ActivityLog, ActivityLogResponseModel>();
    }
}

