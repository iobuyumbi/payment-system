using Solidaridad.Application.Models;
using Solidaridad.Application.Models.Village;
using System.Collections.ObjectModel;

namespace Solidaridad.Application.Services;

public interface IAdminLevel4Service
{
    Task<ReadOnlyCollection<AdminLevel4ResponseModel>> GetAllAsync();
    Task<CreateAdminLevel4ResponseModel> CreateAsync(CreateAdminLevel4Model createVillageModel);

    Task<BaseResponseModel> DeleteAsync(Guid id);

    Task<UpdateAdminLevel4ResponseModel> UpdateAsync(Guid id, UpdateAdminLevel4Model updateVillageModel);
}
