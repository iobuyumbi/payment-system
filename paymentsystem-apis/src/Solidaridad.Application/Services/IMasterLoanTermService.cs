using Solidaridad.Application.Models;
using Solidaridad.Application.Models.LoanTerm;
using Solidaridad.Core.Entities.Base;

namespace Solidaridad.Application.Services;

public interface IMasterLoanTermService
{
    Task<CreateMasterLoanTermResponseModel> CreateAsync(CreateMasterLoanTermModel createLoanTermModel);

    Task<BaseResponseModel> DeleteAsync(Guid id);
    
    Task<IEnumerable<MasterLoanTermResponseModel>> GetAllAsync(SearchParams searchParams);

    Task<MasterLoanTermResponseModel> GetByIdAsync(Guid id, Guid countryId);

    Task<UpdateMasterLoanTermResponseModel> UpdateAsync(Guid id, UpdateMasterLoanTermModel updateLoanTermModel);
}
