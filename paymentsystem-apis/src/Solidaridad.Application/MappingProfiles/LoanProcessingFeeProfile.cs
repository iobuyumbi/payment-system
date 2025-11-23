using AutoMapper;
using Solidaridad.Application.Models.LoanProcessingFee;
using Solidaridad.Core.Entities;

namespace Solidaridad.Application.MappingProfiles;

public class LoanProcessingFeeProfile : Profile
{
    public LoanProcessingFeeProfile()
    {
        CreateMap<CreateLoanProcessingFeeModel, MasterLoanTermAdditionalFee>();

        CreateMap<MasterLoanTermAdditionalFee, LoanProcessingFeeResponseModel>();

        CreateMap<UpdateLoanProcessingFeeModel, MasterLoanTermAdditionalFee>();
    }
}
