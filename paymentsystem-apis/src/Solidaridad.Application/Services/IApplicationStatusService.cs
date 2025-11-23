using Solidaridad.Application.Models;
using Solidaridad.Application.Models.ApplicationStatus;
using Solidaridad.Core.Entities.Base;
using System.Collections.ObjectModel;

namespace Solidaridad.Application.Services;

public interface IApplicationStatusService
{
    Task<ReadOnlyCollection<ApplicationStatusResponseModel>> GetAllAsync(ApplicationStatusSearchParams searchParams);
    Task<CreateApplcationStatusResponseModel> CreateAsync(CreateApplicationStatusModel createCountryModel);

    Task<BaseResponseModel> DeleteAsync(Guid id);

    Task<UpdateApplicationStatusResponseModel> UpdateAsync(Guid id, UpdateApplicationStatusModel updateCountryModel);
}
