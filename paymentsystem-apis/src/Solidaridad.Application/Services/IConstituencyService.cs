using Solidaridad.Application.Models.Constituency;
using System.Collections.ObjectModel;

namespace Solidaridad.Application.Services;

public interface IConstituencyService
{
    Task<ReadOnlyCollection<ConstituencyResponseModel>> GetAllAsync();
}
