using Microsoft.EntityFrameworkCore;
using Solidaridad.Core.Entities;
using Solidaridad.Core.Entities.Loans;
using Solidaridad.DataAccess.Persistence;

namespace Solidaridad.DataAccess.Repositories.Impl;

public class LoanBatchRepository : BaseRepository<LoanBatch>, ILoanBatchRepository
{
    #region DI
    protected readonly DbSet<Project> ProjectSet;
    protected readonly DbSet<LoanBatch> LoanBatchSet;
    protected readonly DbSet<LoanBatchProcessingFee> LoanBatchProcessingFee;

    public LoanBatchRepository(DatabaseContext context) : base(context)
    {
        ProjectSet = context.Set<Project>();
        LoanBatchSet = context.Set<LoanBatch>();
        LoanBatchProcessingFee = context.Set<LoanBatchProcessingFee>();
    }
    #endregion

    #region Methods
    public object GetByProjectIds(List<string> projectIds)
    {
        var query = LoanBatchSet
                                .Where(loanBatch => projectIds.Contains(loanBatch.ProjectId.ToString()))
                                .Select(loanBatch => new
                                {
                                    LoanBatch = loanBatch,
                                    Project = loanBatch.Project
                                });


        // Execute the query to get the result
        return query.ToList();
    }

    public async Task<LoanBatch> GetSingle(Guid id)
    {
        return await LoanBatchSet
                                 .Where(c => c.Id == id)
                                 .Include(c => c.ProcessingFees)
                                 .FirstOrDefaultAsync();
    }

    public int GetProjectCount(Guid countryId)
    {
        return ProjectSet.Count(c=> c.CountryId == countryId && !c.IsDeleted);
    }

    public int GetLoanBatchCount(Guid countryId)
    {
        var projectIds = ProjectSet
            .Where(c => c.CountryId == countryId && !c.IsDeleted)
            .Select(p => p.Id)
            .ToList();

        return LoanBatchSet.Count(lb => projectIds.Contains(lb.ProjectId) && !lb.IsDeleted);
    }

    public async Task AddLoanBatchProcessingFeeAsync(LoanBatchProcessingFee loanBatchProcessingFee)
    {
        await LoanBatchProcessingFee.AddAsync(loanBatchProcessingFee);
    }

    public async Task AddRangeLoanBatchProcessingFeeAsync(List<LoanBatchProcessingFee> loanBatchProcessingFees)
    {
        try
        {
            await LoanBatchProcessingFee.AddRangeAsync(loanBatchProcessingFees);
            Context.SaveChanges();
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    public async Task<LoanBatchProcessingFee> DeleteByBatchId(LoanBatchProcessingFee item)
    {
        try
        {
            LoanBatchProcessingFee.Remove(item);  
            await Context.SaveChangesAsync();      
            return item;
        }
        catch (Exception ex)
        {
            throw; 
        }
    }
    #endregion
}
