using Solidaridad.Application.Models;
using Solidaridad.Application.Models.County;
using Solidaridad.Core.Entities.Base;
using System.Collections.ObjectModel;

namespace Solidaridad.Application.Services;

public interface IAdminLevel1Service
{
    Task<ReadOnlyCollection<AdminLevel1ResponseModel>> GetAllAsync(AdminLevel1SearchParams searchParams);
    Task<CreateAdminLevel1ResponseModel> CreateAsync(CreateAdminLevel1Model createCountryModel);

    Task<BaseResponseModel> DeleteAsync(Guid id);

    Task<UpdateAdminLevel1ResponseModel> UpdateAsync(Guid id, UpdateAdminLevel1Model updateCountryModel);
}
