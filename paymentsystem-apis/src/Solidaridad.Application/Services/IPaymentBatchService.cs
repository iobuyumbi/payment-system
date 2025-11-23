using Solidaridad.Application.Models;
using Solidaridad.Application.Models.PaymentBatch;
using Solidaridad.Application.Models.PaymentBatch.Transitions;
using Solidaridad.Core.Entities;
using Solidaridad.Core.Entities.Base;
using Solidaridad.Core.Entities.Payments;
using Solidaridad.Core.Enums;

namespace Solidaridad.Application.Services;

public interface IPaymentBatchService 
{
    Task<CreatePaymentBatchResponseModel> CreateAsync(CreatePaymentBatchModel model);

    Task<BaseResponseModel> DeleteAsync(Guid id);

    Task<PaymentBatchesResponseModel> GetAllAsync(PaymentSearchParams searchParams);
    
    Task<PaymentStatsResponseModel> GetStats(Guid? CountryId);
    
    Task<object> GetPaymentBatchHistory(Guid id);
    
    Task<UpdatePaymentBatchResponseModel> UpdateAsync(Guid id, UpdatePaymentBatchModel model);
    
    Task<UpdatePaymentBatchResponseModel> UpdateStage(Guid id, UpdateStageModel model);
    
    Task<PaymentBatchResponseModel> GetSingle(Guid id, Guid? countryId);

    Task<bool> SendEmail(Guid batchId);
    
    Task<UpdatePaymentBatchResponseModel> ProcessManually(Guid id);
    
    EffectiveRole GetEffectiveRole(PaymentBatchResponseModel batch);
    
    TransitionResponse GetActionContext(PaymentBatchResponseModel batch);
    
    bool TryTransition(PaymentBatchResponseModel batch, TransitionRequest req, out string error);
}
