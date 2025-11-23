using Microsoft.AspNetCore.Http;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.ExcelExport;
using Solidaridad.Application.Models.ExcelImport;
using Solidaridad.Application.Models.Farmer;
using Solidaridad.Core.Entities.Base;

namespace Solidaridad.Application.Services;

public interface IFarmerService
{
    Task<CreateFarmerResponseModel> CreateAsync(CreateFarmerModel createFarmerModel);

    Task<BaseResponseModel> DeleteAsync(Guid id);

    Task<FarmerSearchResponseModel> GetAllAsync(FarmerSearchParams farmerSearchParams);
    Task<IEnumerable<PaymentRequestDeductibleModel>> GetAllByBatchAsync(DeductibleExportModel model);
    Task<UpdateFarmerResponseModel> UpdateAsync(Guid id, UpdateFarmerModel updateFarmerModel);

    Task<IEnumerable<FarmerResponseModel>>GetAllByListIdAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task ImportFarmer(IFormFile file, Guid? id);

    Task<ImportFarmerResponseModel> ImportAsync(ImportFarmerModel importFarmerModel);
    
    Task<ImportFarmerResponseModel> ImportByApiAsync(ImportFarmerModel importFarmerModel);
    
    Task<IEnumerable<SelectItemModel>> GetCooperatives(Guid? farmerId);
    Task<IEnumerable<SelectItemModel>> GetMasterDocumentTypes();
    Task<IEnumerable<SelectItemModel>> GetProjects(Guid farmerId);
}
