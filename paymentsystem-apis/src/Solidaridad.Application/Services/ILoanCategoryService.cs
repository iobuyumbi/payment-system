using Solidaridad.Application.Models.ItemCategory;
using Solidaridad.Application.Models;
using Solidaridad.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Solidaridad.Application.Models.LoanCategory;

namespace Solidaridad.Application.Services.Impl;

public interface ILoanCategoryService
{
    Task<CreateLoanCategoryResponseModel> CreateAsync(CreateLoanCategoryModel loanCategoryModel);

    Task<BaseResponseModel> DeleteAsync(Guid id);

    Task<IEnumerable<LoanCategoryResponseModel>> GetAllAsync(LoanCategorySearchParams searchParams);

    Task<UpdateLoanCategoryResponseModel> UpdateAsync(Guid id, UpdateLoanCategoryModel loanCategoryModel);
}
