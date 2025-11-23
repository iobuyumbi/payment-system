using Microsoft.EntityFrameworkCore;
using Solidaridad.Core.Entities;
using Solidaridad.Core.Entities.Loans;
using Solidaridad.DataAccess.Persistence;
using System.Linq.Expressions;

namespace Solidaridad.DataAccess.Repositories.Impl;

public class LoanApplicationRepository : BaseRepository<LoanApplication>, ILoanApplicationRepository
{
    #region DI
    protected readonly DbSet<EMISchedule> EMISchedules;
    protected readonly DbSet<MasterLoanAppStage> LoanAppStage;
    protected readonly DbSet<LoanApplicationHistory> LoanApplicationHistory;
    protected readonly DbSet<LoanApplication> LoanApplication;
    protected readonly DbSet<LoanItem> LoanItem;
    protected readonly DbSet<LoanApplicationImportStaging> LoanApplicationStaging;
    protected readonly DbSet<LoanItemImportStaging> LoanItemStaging;

    public LoanApplicationRepository(DatabaseContext context) : base(context)
    {
        EMISchedules = context.Set<EMISchedule>();
        LoanAppStage = context.Set<MasterLoanAppStage>();
        LoanApplicationStaging = context.Set<LoanApplicationImportStaging>();
        LoanApplication = context.Set<LoanApplication>();
        LoanItemStaging = context.Set<LoanItemImportStaging>();
        LoanItem = context.Set<LoanItem>();
    }
    #endregion

    #region Methods
    public void SaveUpdatedSchedule(Guid loanApplicationId, List<EMISchedule> updatedSchedule)
    {
        var existingRecords = EMISchedules.Where(e => e.LoanApplicationId == loanApplicationId).ToList();

        foreach (var emi in updatedSchedule)
        {
            var existingEmi = existingRecords.FirstOrDefault(e => e.Id == emi.Id);
            if (existingEmi != null)
            {
                existingEmi.PrincipalAmount = emi.PrincipalAmount;
                existingEmi.Balance = emi.Balance;
                existingEmi.InterestAmount = emi.InterestAmount;
                existingEmi.PaymentStatus = emi.PaymentStatus;

                EMISchedules.Update(existingEmi);
            }

            Context.SaveChanges();
        }
    }

    public async Task<IEnumerable<MasterLoanAppStage>> GetLoanAppApprovalStages()
    {
        return await LoanAppStage.Where(c => c.IsDeleted == false).ToListAsync();
    }

    public async Task AddLoanAppHistory(LoanApplicationHistory loanApplicationHistory)
    {
        await LoanApplicationHistory.AddAsync(loanApplicationHistory);
    }

    public async Task<List<LoanApplication>> GetFull(Expression<Func<LoanApplication, bool>> predicate)
    {
        return await LoanApplication
                                 .Where(predicate)
                                 .Include(c => c.Farmer)
                                 .ToListAsync();
    }

    public void UpdateLoanAppImportStaging(LoanApplicationImportStaging loanApplicationImportStaging)
    {
        LoanApplicationStaging.Update(loanApplicationImportStaging);
    }

    public async Task<IEnumerable<LoanApplicationImportStaging>> GetLoanAppImportStaging(Guid batchId)
    {
        return await LoanApplicationStaging.Where(c => c.LoanBatchId == batchId).ToListAsync();
    }

    public async Task TransferFromStaging(Guid excelImportId)
    {
        // 1. Fetch approved staging applications
        var approvedApplications = await LoanApplicationStaging
            .Where(x => x.StatusId == 1 && x.ExcelImportId == excelImportId)
            .ToListAsync();

        if (!approvedApplications.Any())
            throw new InvalidOperationException("No approved loan applications found in staging.");

        // 2. Create new LoanApplications
        var loanApplications = approvedApplications.Select(app => new LoanApplication
        {
            Id = app.Id,
            FarmerId = app.FarmerId!.Value,
            WitnessFullName = app.WitnessFullName,
            WitnessNationalId = app.WitnessNationalId,
            WitnessPhoneNo = app.WitnessPhoneNo,
            WitnessRelation = app.WitnessRelation,
            DateOfWitness = app.DateOfWitness,
            EnumeratorFullName = app.EnumeratorFullName,
            LoanBatchId = app.LoanBatchId,
            Status = app.Status,
            CreatedBy = app.CreatedBy,
            CreatedOn = app.CreatedOn,
            UpdatedBy = null,
            UpdatedOn = null,
            PrincipalAmount = app.PrincipalAmount,
            EffectivePrincipal = app.EffectivePrincipal,
            AccruedInterest = app.AccruedInterest,
            InterestAmount = app.InterestAmount,
            InterestRate = app.InterestRate,
            LoanNumber = app.LoanNumber,
            RemainingBalance = app.RemainingBalance,
            FeeApplied = app.FeeApplied
        }).ToList();

        // 3. Save LoanApplications first so FKs are valid
        await LoanApplication.AddRangeAsync(loanApplications);
        await Context.SaveChangesAsync();

        // 4. Build a quick lookup to verify application IDs
        var validAppIds = loanApplications.Select(l => l.Id).ToHashSet();

        // 5. Fetch approved LoanItem staging records
        var approvedItems = await LoanItemStaging
            .Where(x => x.StatusId == 1 && x.ExcelImportId == excelImportId)
            .ToListAsync();

        // 6. Filter items whose LoanApplicationId exists in the saved applications
        var validItems = approvedItems
            .Where(item => item.LoanApplicationId.HasValue && validAppIds.Contains(item.LoanApplicationId.Value))
            .ToList();

        // 7. Warn if any items are invalid
        var invalidItems = approvedItems.Except(validItems).ToList();
        if (invalidItems.Any())
        {
            // Log or notify skipped items
            Console.WriteLine($"Skipped {invalidItems.Count} item(s) due to missing LoanApplication references.");
        }

        // 8. Map to LoanItem entities
        var loanItems = validItems.Select(item => new LoanItem
        {
            Id = item.Id,
            ItemName = item.ItemName,
            LoanApplicationId = item.LoanApplicationId!.Value,
            MasterLoanItemId = item.MasterLoanItemId!.Value,
            Quantity = item.Quantity,
            UnitId = item.UnitId ?? 0,
            UnitPrice = item.UnitPrice
        }).ToList();

        // 9. Save LoanItems
        await LoanItem.AddRangeAsync(loanItems);
        await Context.SaveChangesAsync();
    }


    public async Task TransferFromStaging_Not_in_use(Guid excelImportId)
    {
        #region Loan Application
        // 1. Fetch staging records with StatusId = 1 and StageText = "Approved"
        var approvedStagings = await LoanApplicationStaging
            .Where(x => x.StatusId == 1 && x.ExcelImportId == excelImportId)
            .ToListAsync();

        // 2. Map them to LoanApplications
        var newLoanApplications = approvedStagings.Select(s => new LoanApplication
        {
            Id = s.Id,
            FarmerId = s.FarmerId.Value,
            WitnessFullName = s.WitnessFullName,
            WitnessNationalId = s.WitnessNationalId,
            WitnessPhoneNo = s.WitnessPhoneNo,
            WitnessRelation = s.WitnessRelation,
            DateOfWitness = s.DateOfWitness,
            EnumeratorFullName = s.EnumeratorFullName,
            LoanBatchId = s.LoanBatchId,
            Status = s.Status, // Make sure this is a GUID matching your domain status enum/id
            CreatedBy = s.CreatedBy,
            CreatedOn = s.CreatedOn, // If you're setting now: DateTime.UtcNow
            UpdatedBy = null, // Assuming this is a new insert
            UpdatedOn = null,
            PrincipalAmount = s.PrincipalAmount,
            EffectivePrincipal = s.EffectivePrincipal,
            AccruedInterest = s.AccruedInterest,
            InterestAmount = s.InterestAmount,
            InterestRate = s.InterestRate,
            LoanNumber = s.LoanNumber,
            RemainingBalance = s.RemainingBalance,
            FeeApplied = s.FeeApplied,
        }).ToList();


        // 3. Add to context and save
        await LoanApplication.AddRangeAsync(newLoanApplications);
        #endregion

        #region Loan Items

        // 4. Fetch staging records with StatusId = 1 and StageText = "Approved"
        var approvedStagingItems = await LoanItemStaging
            .Where(x => x.StatusId == 1 && x.ExcelImportId == excelImportId)
            .ToListAsync();

        // 5. Map them to LoanItems
        var newLoanItems = approvedStagingItems.Select(s => new LoanItem
        {
            Id = s.Id,
            ItemName = s.ItemName,
            LoanApplicationId = (Guid)s.LoanApplicationId,
            MasterLoanItemId = (Guid)s.MasterLoanItemId,
            Quantity = s.Quantity,
            UnitId = s.UnitId ?? 0,
            UnitPrice = s.UnitPrice,
        }).ToList();

        // 6. Add to context and save
        await LoanItem.AddRangeAsync(newLoanItems);

        #endregion

        await Context.SaveChangesAsync();
    }
    #endregion
}
