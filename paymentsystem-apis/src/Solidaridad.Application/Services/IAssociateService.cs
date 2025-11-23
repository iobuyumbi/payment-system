using Solidaridad.Application.Models.AdminLevels;
using Solidaridad.Application.Models.Country;
using Solidaridad.Application.Models;
using System.Collections.ObjectModel;
using Solidaridad.Application.Models.Associate;
using Solidaridad.Application.Models.LoanApplication;
using Solidaridad.Application.Models.Farmer;

namespace Solidaridad.Application.Services;

public interface IAssociateService
{
    Task<ReadOnlyCollection<AssociateResponseModel>> GetAllAsync();
    Task<ReadOnlyCollection<FarmerResponseModel>> GetAssociatedFarmers(Guid batchId );

    Task<CreateAssociateResponseModel> CreateAsync(CreateAssociateModel createCountryModel);

    Task<IEnumerable<CreateAssociateResponseModel>> MultiAdd(IEnumerable<CreateAssociateModel> createCountryModel);

    Task<BaseResponseModel> DeleteAsync(Guid id);

    Task<UpdateAssociateResponseModel> UpdateAsync(Guid id, UpdateAssociateModel updateCountryModel);

}
