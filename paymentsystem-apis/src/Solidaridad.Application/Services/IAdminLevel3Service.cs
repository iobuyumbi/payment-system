using Solidaridad.Application.Models;
using Solidaridad.Application.Models.Ward;
using Solidaridad.Core.Entities.Base;
using System.Collections.ObjectModel;

namespace Solidaridad.Application.Services;

public interface IAdminLevel3Service
{
    Task<ReadOnlyCollection<AdminLevel3ResponseModel>> GetAllAsync(AdminLevel3SearchParams searchParams);

    Task<CreateAdminLevel3ResponseModel> CreateAsync(CreateAdminLevel3Model createWardModel);

    Task<BaseResponseModel> DeleteAsync(Guid id);

    Task<UpdateAdminLevel3ResponseModel> UpdateAsync(Guid id, UpdateAdminLevel3Model updateWardModel);
}
