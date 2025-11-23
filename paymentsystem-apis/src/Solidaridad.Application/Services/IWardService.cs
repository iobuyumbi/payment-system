using Solidaridad.Application.Models.Ward;
using System.Collections.ObjectModel;

namespace Solidaridad.Application.Services;

public interface IWardService
{
    Task<ReadOnlyCollection<WardResponseModel>> GetAllAsync();
}
