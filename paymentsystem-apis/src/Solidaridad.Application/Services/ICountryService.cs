using Solidaridad.Application.Models;
using Solidaridad.Application.Models.AdminLevels;
using Solidaridad.Application.Models.Country;
using System.Collections.ObjectModel;

namespace Solidaridad.Application.Services;

public interface ICountryService
{
    Task<ReadOnlyCollection<CountryResponseModel>> GetAllWithCountryIdAsync(Guid? countryId);
    Task<ReadOnlyCollection<CountryResponseModel>> GetAllAsync();
    Task<AdminLevelResponseModel> GetAllAdminLevels(string countryName);
    
    Task<CreateCountryResponseModel> CreateAsync(CreateCountryModel createCountryModel);

    Task<BaseResponseModel> DeleteAsync(Guid id);

    Task<UpdateCountryResponseModel> UpdateAsync(Guid id, UpdateCountryModel updateCountryModel);
}
