using Microsoft.EntityFrameworkCore;
using Solidaridad.Core.Entities;
using Solidaridad.Core.Entities.Loans;
using Solidaridad.DataAccess.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solidaridad.DataAccess.Repositories.Impl;

public class LoanStatementRepository : BaseRepository<LoanStatement>, ILoanStatementRepository
{
    public LoanStatementRepository (DatabaseContext context) : base(context) { }


    public async Task DeleteLoanStatement(Guid id)
    {
        await Context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM \"LoanStatement\" WHERE \"Id\" = {id}");

    }
}
