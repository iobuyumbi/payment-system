using Solidaridad.Core.Entities;

namespace Solidaridad.DataAccess.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<List<UserCountry>> AddUserCountryAsync(string userId, List<Guid> countryIds);

    Task<string> GetUserCountriesStr(string userId);

    Task<IEnumerable<Country>> GetUserCountries(string userId);

    Task<List<UserCountry>> UpdateUserCountryAsync(string userId, List<Guid> countryIds);
    
    Task<bool> VerifyOtpAsync(string userId, string enteredOtp);
    
    Task<string> CreateAndStoreOtpAsync(string userId);
}
