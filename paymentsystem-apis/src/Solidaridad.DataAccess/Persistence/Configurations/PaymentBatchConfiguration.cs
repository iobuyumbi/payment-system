using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Solidaridad.Core.Entities.Payments;

namespace Solidaridad.DataAccess.Persistence.Configurations;

public class PaymentBatchConfiguration : IEntityTypeConfiguration<PaymentBatch>
{
    public void Configure(EntityTypeBuilder<PaymentBatch> builder)
    {
        builder.Property(ti => ti.BatchName)
               .HasMaxLength(126)
                   .IsRequired();

        builder.Property(ti => ti.DateCreated)
           .IsRequired();

        builder.Property(ti => ti.CountryId)
           .IsRequired();

        builder.Property(ti => ti.ProjectId)
          .IsRequired();

        builder.Property(e => e.DateCreated)
            .HasDefaultValueSql("NOW()"); // PostgreSQL
    }
}
