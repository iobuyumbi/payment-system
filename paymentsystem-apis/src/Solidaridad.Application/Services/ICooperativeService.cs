using Solidaridad.Application.Models;
using Solidaridad.Application.Models.Cooperative;
using Solidaridad.Core.Entities.Base;

namespace Solidaridad.Application.Services;

public interface ICooperativeService
{
    Task<CreateCooperativeResponseModel> CreateAsync(CreateCooperativeModel createCooperativeModel);

    Task<BaseResponseModel> DeleteAsync(Guid id);

    Task<IEnumerable<CooperativeResponseModel>> GetAllAsync(CooperativeSearchParams cooperativeSearchParams);
    Task<CreateCooperativeResponseModel> ImportAsync(ImportCoperativeModel importCoperativeModel);
    Task<UpdateCooperativeResponseModel> UpdateAsync(Guid id, UpdateCooperativeModel updateCooperativeModel);
}
