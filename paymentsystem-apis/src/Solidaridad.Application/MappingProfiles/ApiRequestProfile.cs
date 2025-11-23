using AutoMapper;
using Solidaridad.Application.Models.ApiRequestLog;
using Solidaridad.Application.Models.Associate;
using Solidaridad.Core.Entities;
using Solidaridad.Core.Entities.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solidaridad.Application.MappingProfiles;

public class ApiRequestProfile : Profile
{
    public ApiRequestProfile()
    {
        CreateMap<ApiRequestLogResponseModel, ApiRequestLog>();
        CreateMap< ApiRequestLog, ApiRequestLogResponseModel>();
    }
}

