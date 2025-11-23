using Solidaridad.Core.Entities;
using Solidaridad.Core.Entities.Loans;
using Solidaridad.DataAccess.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solidaridad.DataAccess.Repositories.Impl;

public class MasterLoanTermsMappingRepository : BaseRepository<MasterLoanTermAdditionalFeeMapping>, IMasterLoanTermsMappingRepository
{
    public MasterLoanTermsMappingRepository(DatabaseContext context) : base(context)
    {
    }
}

