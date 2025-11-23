using AutoMapper;
using Solidaridad.Application.Models.EmiSchedule;
using Solidaridad.Core.Entities;

namespace Solidaridad.Application.MappingProfiles;

public class EMIScheduleProfile : Profile
{
    public EMIScheduleProfile()
    {
        CreateMap<EMISchedule, EMIScheduleResponseModel>();
    }
}
