using Solidaridad.Application.Models.County;
using System.Collections.ObjectModel;

namespace Solidaridad.Application.Services;

public interface ICountyService
{
    Task<ReadOnlyCollection<CountyResponseModel>> GetAllAsync();
}
