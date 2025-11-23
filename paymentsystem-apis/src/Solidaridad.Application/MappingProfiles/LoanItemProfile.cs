using AutoMapper;
using Solidaridad.Application.Models.LoanApplication;
using Solidaridad.Application.Models.LoanBatch;
using Solidaridad.Application.Models.LoanItem;
using Solidaridad.Core.Entities.Loans;

namespace Solidaridad.Application.MappingProfiles;

public class LoanItemProfile : Profile
{
    public LoanItemProfile()
    {
        CreateMap<CreateMasterLoanItemModel, MasterLoanItem>();

        CreateMap<MasterLoanItem, MasterLoanItemResponseModel>();

        CreateMap<UpdateMasterLoanItemModel, MasterLoanItem>();

        CreateMap<MasterLoanItemResponseModel, MasterLoanItem>();

        CreateMap<CreateLoanBatchItemModel, LoanBatchItem>()
            .ForMember(dest => dest.LoanBatch, opt => opt.Ignore())
            .ForMember(dest => dest.LoanItem, opt => opt.Ignore())
            .ForMember(dest => dest.Unit, opt => opt.Ignore())
            .ForPath(c => c.LoanItemId, c => c.MapFrom(cti => cti.LoanItem.Value))
            .ForPath(c => c.LoanBatchId, c => c.MapFrom(cti => cti.LoanBatch.Value))
            .ForPath(c => c.UnitId, c => c.MapFrom(cti => cti.Unit.Value));

        CreateMap<UpdateLoanBatchItemModel, LoanBatchItem>()
           .ForMember(dest => dest.LoanBatch, opt => opt.Ignore())
           .ForMember(dest => dest.LoanItem, opt => opt.Ignore())
           .ForMember(dest => dest.Unit, opt => opt.Ignore())
           .ForPath(c => c.LoanItemId, c => c.MapFrom(cti => cti.LoanItem.Value))
           .ForPath(c => c.LoanBatchId, c => c.MapFrom(cti => cti.LoanBatch.Value))
           .ForPath(c => c.UnitId, c => c.MapFrom(cti => cti.Unit.Value));

        CreateMap<LoanBatchItem, LoanBatchItemResponseModel>()
           .ForMember(dest => dest.LoanBatch, opt => opt.Ignore())
           .ForMember(dest => dest.LoanItem, opt => opt.Ignore())
           .ForMember(dest => dest.Unit, opt => opt.Ignore());

        CreateMap<LoanItem, LoanAppItemImportModel>();
        CreateMap<LoanAppItemImportModel, LoanItem>();
        CreateMap<LoanItem, LoanItemResponseModel>();
        CreateMap<LoanItemResponseModel, LoanItem>();
        CreateMap<LoanItem, CreateLoanItemModel>();
        CreateMap<CreateLoanItemModel, LoanItem>();
        CreateMap<LoanItem, UpdateLoanItemModel>();
        CreateMap<UpdateLoanItemModel, LoanItem>();
    }
}
