using AutoMapper;
using Solidaridad.Application.Models.LoanCategory;
using Solidaridad.Core.Entities.Loans;

namespace Solidaridad.Application.MappingProfiles;

public class LoanCategoryProfile : Profile
{
    public LoanCategoryProfile()
    {
        CreateMap<CreateLoanCategoryModel, LoanCategory>();
        
        CreateMap<LoanCategory, LoanCategoryResponseModel>();
        
        CreateMap<LoanCategory, CreateLoanCategoryModel>();
        
        CreateMap<UpdateLoanCategoryModel, LoanCategory>();
     
        CreateMap<LoanCategoryResponseModel, LoanCategory>();
    }
}
