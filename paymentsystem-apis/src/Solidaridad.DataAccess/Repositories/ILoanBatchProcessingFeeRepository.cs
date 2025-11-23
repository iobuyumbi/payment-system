using Solidaridad.Core.Entities;
using Solidaridad.Core.Entities.Loans;

namespace Solidaridad.DataAccess.Repositories;

public interface ILoanBatchProcessingFeeRepository : IBaseRepository<LoanBatchProcessingFee>
{
    Task DeleteLoanBatchProcessingFee(Guid id);
}

