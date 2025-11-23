using Solidaridad.Application.Models;
using Solidaridad.Application.Models.Location;
using Solidaridad.Core.Entities.Base;

namespace Solidaridad.Application.Services;

public interface ILocationService
{
    Task<CreateLocationResponseModel> CreateAsync(CreateLocationModel createLocationModel);

    Task<BaseResponseModel> DeleteAsync(Guid id);
    
    Task<IEnumerable<LocationResponseModel>> GetAllAsync(SearchParams searchParams);

    Task<LocationResponseModel> GetByIdAsync(Guid id, Guid countryId);

    Task<UpdateLocationResponseModel> UpdateAsync(Guid id, UpdateLocationModel updateLocationModel);
}
