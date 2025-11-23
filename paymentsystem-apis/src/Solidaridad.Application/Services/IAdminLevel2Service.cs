using Solidaridad.Application.Models;
using Solidaridad.Application.Models.SubCounty;
using Solidaridad.Core.Entities.Base;
using System.Collections.ObjectModel;

namespace Solidaridad.Application.Services;

public interface IAdminLevel2Service
{
    Task<ReadOnlyCollection<AdminLevel2ResponseModel>> GetAllAsync(AdminLevel2SearchParams searchParams);
    Task<CreateAdminLevel2ResponseModel> CreateAsync(CreateAdminLevel2Model createSubCountryModel);

    Task<BaseResponseModel> DeleteAsync(Guid id);

    Task<UpdateAdminLevel2ResponseModel> UpdateAsync(Guid id, UpdateAdminLevel2Model updateSubCountryModel);
}
