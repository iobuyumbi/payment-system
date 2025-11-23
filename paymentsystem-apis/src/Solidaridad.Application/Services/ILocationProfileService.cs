using Solidaridad.Application.Models;
using Solidaridad.Application.Models.LocationProfiles;
using Solidaridad.Core.Entities.Base;

namespace Solidaridad.Application.Services;

public interface ILocationProfileService
{
    Task<CreateLocationProfileResponseModel> CreateAsync(CreateLocationProfileModel createLocationProfileModel);

    Task<BaseResponseModel> DeleteAsync(Guid id);
    
    Task<IEnumerable<LocationProfileResponseModel>> GetAllAsync(SearchParams searchParams);

    Task<LocationProfileResponseModel> GetByIdAsync(Guid id);

    Task<UpdateLocationProfileResponseModel> UpdateAsync(Guid id, UpdateLocationProfileModel updateLocationModel);
}
