using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Solidaridad.Core.Entities.Loans;

namespace Solidaridad.DataAccess.Persistence.Configurations;

public class LoanBatchItemConfiguration : IEntityTypeConfiguration<LoanBatchItem>
{
    public void Configure(EntityTypeBuilder<LoanBatchItem> builder)
    {
         builder.Property(ti => ti.SupplierDetails)
            .HasMaxLength(1000)
            .IsRequired(false);

        builder.Property(ti => ti.Quantity)
            .HasPrecision(18,2)
            .IsRequired();

        builder.Property(ti => ti.UnitPrice)
            .HasPrecision(18,2)
            .IsRequired(false);
    }
}
