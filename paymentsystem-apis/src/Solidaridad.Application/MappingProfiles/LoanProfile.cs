using AutoMapper;
using Solidaridad.Application.Models.LoanApplication;
using Solidaridad.Application.Models.LoanRepayment;
using Solidaridad.Core.Entities.Loans;

namespace Solidaridad.Application.MappingProfiles;

public class LoanProfile : Profile
{
    public LoanProfile()
    {
        CreateMap<LoanApplication, CreateLoanApplicationModel>();

        CreateMap<LoanApplication, UpdateLoanApplicationModel>();

        CreateMap<LoanApplicationResponseModel, LoanApplication>();

        CreateMap<LoanApplication, LoanApplicationResponseModel>();

        CreateMap<LoanRepayment, LoanRepaymentResponseModel>();

        CreateMap<CreateLoanApplicationModel, LoanApplication>()
            .ForMember(dest => dest.Farmer, opt => opt.Ignore())
            .ForMember(dest => dest.LoanItems, opt => opt.Ignore())
             .ForMember(dest => dest.OfficerId, opt => opt.Ignore())
            .ForPath(c => c.FarmerId, c => c.MapFrom(cti => cti.Farmer.Value));

        CreateMap<UpdateLoanApplicationModel, LoanApplication>().ForMember(dest => dest.Farmer, opt => opt.Ignore())
            .ForMember(dest => dest.LoanItems, opt => opt.Ignore())
            .ForMember(dest => dest.OfficerId, opt => opt.Ignore())
            .ForPath(c => c.FarmerId, c => c.MapFrom(cti => cti.Farmer.Value));

        CreateMap<ImportLoanApplicationModel, LoanApplication>()
            .ForMember(dest => dest.LoanItems, opt => opt.Ignore())
            .ForMember(dest => dest.Farmer, opt => opt.Ignore());
    }
}
