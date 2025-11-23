using Solidaridad.Core.Entities;
using Solidaridad.Core.Entities.Loans;
using System.Linq.Expressions;

namespace Solidaridad.DataAccess.Repositories;

public interface ILoanApplicationRepository : IBaseRepository<LoanApplication>
{
    Task AddLoanAppHistory(LoanApplicationHistory loanApplicationHistory);
    Task<List<LoanApplication>> GetFull(Expression<Func<LoanApplication, bool>> predicate);

    //Task AddLoanAppImportStaging(List<LoanApplicationImportStaging> loanApplicationImportStaging);
    Task<IEnumerable<MasterLoanAppStage>> GetLoanAppApprovalStages();
    Task<IEnumerable<LoanApplicationImportStaging>> GetLoanAppImportStaging(Guid batchId);
    void SaveUpdatedSchedule(Guid loanApplicationId, List<EMISchedule> updatedSchedule);
    Task TransferFromStaging(Guid excelImportId);
    void UpdateLoanAppImportStaging(LoanApplicationImportStaging loanApplicationImportStaging);
}

