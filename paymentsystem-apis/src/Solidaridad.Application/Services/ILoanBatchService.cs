using Solidaridad.Application.Models;
using Solidaridad.Application.Models.AttachmentUpload;
using Solidaridad.Application.Models.LoanApplication;
using Solidaridad.Application.Models.LoanBatch;
using Solidaridad.Application.Models.PaymentBatch;
using Solidaridad.Core.Entities.Base;

namespace Solidaridad.Application.Services;

public interface ILoanBatchService 
{
    Task<CreateLoanBatchResponseModel> CreateAsync(CreateLoanBatchModel createLoanBatchModel);

    Task<BaseResponseModel> DeleteAsync(Guid id);

    Task<LoanBatchesResponseModel> GetAllAsync(LoanBatchSearchParams loanBatchSearchParams);

    Task<UpdateLoanBatchResponseModel> UpdateAsync(Guid id, UpdateLoanBatchModel updateLoanBatchModel);

    Task<IEnumerable<LoanBatchResponseModel>> GetAllByListIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<CreateLoanBatchItemResponseModel> CreateLoanBatchItem(CreateLoanBatchItemModel createLoanBatchItemModel);
    
    Task<UpdateLoanBatchItemResponseModel> UpdateLoanBatchItemAsync(Guid id, UpdateLoanBatchItemModel updateLoanBatchItemModel);
    
    Task<IEnumerable<SelectItemModel>> GetItemUnitsAsync();

    Task<IEnumerable<LoanBatchItemResponseModel>> GetBatchItems(Guid loanBatchId, CancellationToken cancellationToken = default);
    
    Task<BaseResponseModel> DeleteBatchItemAsync(Guid id);
    
    object GetByProjectIds(List<string> projectIds, CancellationToken cancellationToken = default);
    
    Task<LoanBatchResponseModel> GetSingle(Guid id);

    Task<UpdateLoanBatchResponseModel> UpdateStage(Guid id, Guid excelImportId, UpdateStageModel model);

    Task<IEnumerable<AttachmentResponseModel>> GetBatchDocuments(Guid id, CancellationToken cancellationToken = default);
    
    Task<List<LoanBatchResponseModel>> GetValidLoanBatches(Guid countryId);
}
