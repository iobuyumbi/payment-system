using Solidaridad.Application.Models;
using Solidaridad.Application.Models.LoanItem;
using Solidaridad.Core.Entities.Base;

namespace Solidaridad.Application.Services;

public interface IMasterLoanItemService
{
    Task<CreateMasterLoanItemResponseModel> CreateAsync(CreateMasterLoanItemModel project);

    Task<BaseResponseModel> DeleteAsync(Guid id);

    Task<IEnumerable<MasterLoanItemResponseModel>> GetAllAsync(LoanItemSearchParams searchParams);

    Task<UpdateMasterLoanItemResponseModel> UpdateAsync(Guid id, UpdateMasterLoanItemModel projectModel);
   
}
