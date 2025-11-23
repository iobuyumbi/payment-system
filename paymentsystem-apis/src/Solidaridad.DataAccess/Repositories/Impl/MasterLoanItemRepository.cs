using Microsoft.EntityFrameworkCore;
using Solidaridad.Core.Entities.Loans;
using Solidaridad.DataAccess.Persistence;
using System.Linq.Expressions;

namespace Solidaridad.DataAccess.Repositories.Impl;

public class MasterLoanItemRepository : BaseRepository<MasterLoanItem>, IMasterLoanItemRepository
{
    protected readonly DbSet<MasterLoanItem> MasterLoanItemSet;

    public MasterLoanItemRepository(DatabaseContext context) : base(context)
    {
        MasterLoanItemSet = context.Set<MasterLoanItem>();
    }

    public async Task<IEnumerable<MasterLoanItem>> GetFullAsync(Expression<Func<MasterLoanItem, bool>> predicate)
    {
        var result = await MasterLoanItemSet.Where(predicate)
           .Include(o => o.Category)
           .ToListAsync();

        return result;
    }
}
