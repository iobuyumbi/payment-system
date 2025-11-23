using Solidaridad.Application.Models.LoanItem;
using Solidaridad.Application.Models;
using Solidaridad.Core.Entities.Base;

namespace Solidaridad.Application.Services;

public interface ILoanItemService
{
    Task<CreateLoanItemResponseModel> CreateAsync(CreateLoanItemModel project);

    Task<BaseResponseModel> DeleteAsync(Guid id);

    Task<IEnumerable<LoanItemResponseModel>> GetAllAsync(LoanItemSearchParams searchParams);

    Task<UpdateLoanItemResponseModel> UpdateAsync(Guid id, UpdateLoanItemModel projectModel);
}
