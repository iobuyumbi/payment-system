using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Solidaridad.Core.Entities;
using Solidaridad.Core.Entities.Payments;

namespace Solidaridad.DataAccess.Persistence.Configurations;

public class DisbursementConfiguration : IEntityTypeConfiguration<Disbursement>
{
    public void Configure(EntityTypeBuilder<Disbursement> builder)
    {
        builder.Property(ti => ti.FarmerId).IsRequired();
        builder.Property(ti => ti.MethodId).IsRequired();
        builder.Property(ti => ti.Amount).IsRequired();
        builder.Property(ti => ti.CurrencyCode).HasMaxLength(3).IsRequired();
        builder.Property(ti => ti.Date).IsRequired();
        builder.Property(ti => ti.StatusId).IsRequired();
    }
}


