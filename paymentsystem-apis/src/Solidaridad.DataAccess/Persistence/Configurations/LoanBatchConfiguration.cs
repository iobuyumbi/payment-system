using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Solidaridad.Core.Entities.Loans;

namespace Solidaridad.DataAccess.Persistence.Configurations;

public class LoanBatchConfiguration : IEntityTypeConfiguration<LoanBatch>
{
    public void Configure(EntityTypeBuilder<LoanBatch> builder)
    {
        builder.Property(ti => ti.Name)
            .HasMaxLength(126)
                .IsRequired();

        builder.Property(ti => ti.InitiatedBy)
            .IsRequired();

        builder.Property(ti => ti.InitiationDate)
           .IsRequired();

        builder.Property(ti => ti.ProjectId)
           .IsRequired();

        builder.Property(ti => ti.StatusId)
           .IsRequired();

        builder.Property(ti => ti.CreatedBy)
            .HasMaxLength(36)
           .IsRequired(true);

        builder.Property(ti => ti.UpdatedBy)
           .HasMaxLength(36)
          .IsRequired(false);
    }
}
