using Microsoft.AspNetCore.Http;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.ApplicationStatusLog;
using Solidaridad.Application.Models.EmiSchedule;
using Solidaridad.Application.Models.LoanApplication;
using Solidaridad.Application.Models.PaymentBatch;
using Solidaridad.Core.Entities;
using Solidaridad.Core.Entities.Base;
using Solidaridad.Core.Entities.Loans;

namespace Solidaridad.Application.Services;

public interface ILoanApplicationService
{
    Task<CreateLoanApplicationResponseModel> CreateAsync(CreateLoanApplicationModel loanApplication);

    Task<BaseResponseModel> DeleteAsync(Guid id);

    Task<IEnumerable<LoanApplicationResponseModel>> GetAllAsync(LoanApplicationSearchParams searchParams);

    Task<UpdateLoanApplicationResponseModel> UpdateAsync(Guid id, UpdateLoanApplicationModel loanApplicationModel);

    Task<ApplicationStatusEditResponseModel> UpdateStatusAsync(Guid statusId, IEnumerable<ApplicationStatusEditModel> statusModel);

    //Task<IEnumerable<LoanApplicationResponseModel>> GetSingleAsync(Guid id, CancellationToken cancellationToken = default);

    Task ImportLoanApplication(IFormFile file, Guid? id, Guid? batchId);

    Task<IEnumerable<LoanApplicationResponseModel>> GetFarmerLoanApps(Guid farmerId, Guid countryId);

    Task<ImportLoanApplicationResponseModel> ImportAsync(ImportLoanApplicationModel loanApplicationModel);

    Task<LoanApplicationResponseModel> GetApplicationDocuments(Guid id, CancellationToken cancellationToken = default);

    Task<IEnumerable<ApplicationStatusLogResponseModel>> GetStatusHistory(Guid id);

    Task<IEnumerable<EMIScheduleResponseModel>> GetEmiSchedule(Guid loanApplicationId);
    
    Task<LoanApplicationResponseModel> GetSingleAsync(Guid id);

    Task<List<EMISchedule>> UpdateEMISchedule(decimal paymentReceived, DateTime paymentDate, Guid loanApplicationId);
    Task<UpdateLoanApplicationResponseModel> UpdateStage(Guid id, UpdateStageModel model);
    Task<LoanApplicationTrackModel> GetImportSummary(Guid loanBatchId);
    Task<List<LoanAppImportModel>> GetImportHistory(Guid loanBatchId);

    Task<IEnumerable<EMISchedule>> GetLatestEMISchedule(Guid id);
    Task<List<LoanApplication>> GetEffectiveLoanApplications(Guid loanBatchId);
}
