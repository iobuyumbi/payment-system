using Solidaridad.Application.Models;
using Solidaridad.Application.Models.Farmer;
using Solidaridad.Core.Entities.Base;

namespace Solidaridad.Application.Services;

public interface IModuleService
{
    Task<CreateModuleResponseModel> CreateAsync(ModuleResponseModel project);

    Task<BaseResponseModel> DeleteAsync(Guid id);

    Task<IEnumerable<ModuleResponseModel>> GetAllAsync(ModuleSearchParams searchParams);

    Task<UpdateModuleResponseModel> UpdateAsync(Guid id, ModuleResponseModel projectModel);
    Task<IEnumerable<ModuleResponseModel>> GetAllByListIdAsync(Guid id, CancellationToken cancellationToken = default);
}
