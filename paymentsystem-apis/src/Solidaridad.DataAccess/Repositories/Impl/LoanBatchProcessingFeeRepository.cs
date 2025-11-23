using Microsoft.EntityFrameworkCore;
using Solidaridad.Core.Entities.Loans;
using Solidaridad.DataAccess.Persistence;

namespace Solidaridad.DataAccess.Repositories.Impl;

public class LoanBatchProcessingFeeRepository : BaseRepository<LoanBatchProcessingFee>, ILoanBatchProcessingFeeRepository
{
    protected readonly DbSet<LoanBatchProcessingFee> LoanBatchProcessingFeeSet;
    public LoanBatchProcessingFeeRepository(DatabaseContext context) : base(context)
    {
        LoanBatchProcessingFeeSet = context.Set<LoanBatchProcessingFee>();
    }

    public async Task DeleteLoanBatchProcessingFee(Guid id)
    {
        await Context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM \"LoanBatchProcessingFee\" WHERE \"LoanBatchId\" = {id}");

    }
}
