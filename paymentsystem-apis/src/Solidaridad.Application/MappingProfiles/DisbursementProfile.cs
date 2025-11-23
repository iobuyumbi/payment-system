using AutoMapper;
using Solidaridad.Application.Models.Disbursement;
using Solidaridad.Core.Entities.Payments;

namespace Solidaridad.Application.MappingProfiles;

public class DisbursementProfile : Profile
{
    public DisbursementProfile()
    {
        CreateMap<Disbursement, DisbursementResponseModel>();

        CreateMap<CreateDisbursementModel, Disbursement>();

        CreateMap<DisbursementResponseModel, Disbursement>();

        CreateMap<UpdateDisbursementModel, Disbursement>();
    }

}
