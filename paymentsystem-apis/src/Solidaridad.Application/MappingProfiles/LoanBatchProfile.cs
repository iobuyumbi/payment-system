using AutoMapper;
using Solidaridad.Application.Models.LoanApplication;
using Solidaridad.Application.Models.LoanTerm;
using Solidaridad.Core.Entities.Loans;

namespace Solidaridad.Application.MappingProfiles;

public class LoanBatchProfile : Profile
{
    public LoanBatchProfile()
    {
        CreateMap<CreateLoanBatchModel, LoanBatch>()
            .ForMember(ti => ti.ProcessingFees, ti => ti.Ignore());

        CreateMap<LoanBatch, LoanBatchResponseModel>();

        CreateMap<LoanBatchResponseModel, LoanBatch>();

        CreateMap<UpdateLoanBatchModel, LoanBatch>();

        CreateMap<LoanBatchResponseModel, LoanBatch>();

        CreateMap<LoanBatchProcessingFee, ProcessingFeesModel>();


        CreateMap<CreateMasterLoanTermModel, MasterLoanTerm>()
           .ForMember(ti => ti.AdditionalFee, ti => ti.Ignore());

        CreateMap<MasterLoanTerm, MasterLoanTermResponseModel>();

        CreateMap<UpdateMasterLoanTermModel, MasterLoanTerm>()
            .ForMember(ti => ti.AdditionalFee, ti => ti.Ignore());

    }
}
