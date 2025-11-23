using Solidaridad.Core.Entities.Loans;

namespace Solidaridad.DataAccess.Repositories;

public interface ILoanBatchRepository : IBaseRepository<LoanBatch>
{
    Task AddLoanBatchProcessingFeeAsync(LoanBatchProcessingFee loanBatchProcessingFee);

    Task AddRangeLoanBatchProcessingFeeAsync(List<LoanBatchProcessingFee> loanBatchProcessingFees);

    Task<LoanBatchProcessingFee> DeleteByBatchId(LoanBatchProcessingFee item);

    object GetByProjectIds(List<string> projectIds);

    int GetLoanBatchCount(Guid countryId);

    int GetProjectCount(Guid countryId);

    Task<LoanBatch> GetSingle(Guid id);
}
