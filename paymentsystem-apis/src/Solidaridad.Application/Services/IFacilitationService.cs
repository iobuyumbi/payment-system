using Solidaridad.Application.Models;
using Solidaridad.Application.Models.Facilitation;
using Solidaridad.Core.Entities.Base;

namespace Solidaridad.Application.Services;

public interface IFacilitationService
{
    Task<IEnumerable<FacilitationResponseModel>> GetAllAsync(FacilitationSearchParams searchParams);

    Task<CreateFacilitationResponseModel> CreateAsync(CreateFacilitationModel createModel);

    Task<BaseResponseModel> DeleteAsync(Guid id);

    Task<UpdateFacilitationResponseModel> UpdateAsync(Guid id, UpdateFacilitationModel updateModel);
}
