using AutoMapper;
using Solidaridad.Application.Models.CallBack;
using Solidaridad.Application.Models.Constituency;
using Solidaridad.Core.Entities;

namespace Solidaridad.Application.MappingProfiles;

internal class CallBackProfile : Profile
{
    public CallBackProfile()
    {
        CreateMap<CallBackRecords, CallBackResponseModel>();

        CreateMap<CreateCallBackModel, CallBackRecords>();
        CreateMap<CreateCallBackRecordsModel, CallBackRecords>();
        CreateMap< CallBackRecords, CreateCallBackRecordsModel>();
        CreateMap<CallBackResponseModel, CallBackRecords>();
    }
}
