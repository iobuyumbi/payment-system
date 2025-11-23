using Solidaridad.Application.Models;
using Solidaridad.Application.Models.Disbursement;
using System.Collections.ObjectModel;

namespace Solidaridad.Application.Services;

public interface IDisbursementService
{
    Task<ReadOnlyCollection<DisbursementResponseModel>> GetAllAsync();

    Task<CreateDisbursementResponseModel> CreateAsync(CreateDisbursementModel createCountryModel);

    Task<BaseResponseModel> DeleteAsync(Guid id);

    Task<UpdateDisbursementResponseModel> UpdateAsync(Guid id, UpdateDisbursementModel updateCountryModel);
}
