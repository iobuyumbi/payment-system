using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Solidaridad.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Solidaridad.Core.Entities.Loans;

namespace Solidaridad.DataAccess.Persistence.Configurations;

public class LoanStatementConfiguration : IEntityTypeConfiguration<LoanStatement>
{
    public void Configure(EntityTypeBuilder<LoanStatement> builder)
    {
       
    }
}
