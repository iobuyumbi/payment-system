using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Solidaridad.Core.Entities.Loans;

namespace Solidaridad.DataAccess.Persistence.Configurations;

public class LoanRepaymentScheduleConfiguration : IEntityTypeConfiguration<LoanRepaymentSchedule>
{
    public void Configure(EntityTypeBuilder<LoanRepaymentSchedule> builder)
    {
        builder.Property(ti => ti.LoanApplicationId)
            .IsRequired();

        builder.Property(ti => ti.FarmerId)
            .IsRequired();

        builder.Property(ti => ti.Period)
           .HasMaxLength(50)
           .IsRequired();

        builder.Property(ti => ti.BeginningBalance)
           .HasPrecision(18, 2)
           .IsRequired();

        builder.Property(ti => ti.Payment)
           .HasPrecision(18, 2)
           .IsRequired();

        builder.Property(ti => ti.Interest)
           .HasPrecision(18, 2)
           .IsRequired();

        builder.Property(ti => ti.Principal)
          .HasPrecision(18, 2)
          .IsRequired();

        builder.Property(ti => ti.EndingBalance)
          .HasPrecision(18, 2)
          .IsRequired();

        builder.Property(ti => ti.Installment)
          .IsRequired();
    }
}
