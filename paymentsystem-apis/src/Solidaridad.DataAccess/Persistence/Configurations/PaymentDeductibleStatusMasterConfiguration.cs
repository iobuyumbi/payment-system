using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Solidaridad.Core.Entities.Loans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Solidaridad.Core.Entities;

namespace Solidaridad.DataAccess.Persistence.Configurations;

public class PaymentDeductibleStatusMasterConfiguration : IEntityTypeConfiguration<PaymentDeductibleStatusMaster>
{
    public void Configure(EntityTypeBuilder<PaymentDeductibleStatusMaster> builder)
    {
        builder.Property(ti => ti.Id)
            .IsRequired();

        builder.Property(ti => ti.Name)
             .HasMaxLength(50)
             .IsRequired();

    
    }
}
