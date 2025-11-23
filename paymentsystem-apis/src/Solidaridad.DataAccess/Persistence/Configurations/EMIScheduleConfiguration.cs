using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Solidaridad.Core.Entities;

namespace Solidaridad.DataAccess.Persistence.Configurations;

public class EMIScheduleConfiguration : IEntityTypeConfiguration<EMISchedule>
{
    public void Configure(EntityTypeBuilder<EMISchedule> builder)
    {
        builder.Property(ti => ti.Amount)
             .HasPrecision(18, 2)
             .IsRequired();

        builder.Property(ti => ti.InterestAmount)
             .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(ti => ti.StartDate)
            .IsRequired();

        builder.Property(ti => ti.EndDate)
            .IsRequired();

        builder.Property(ti => ti.Balance)
             .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(ti => ti.LoanApplicationId)
            .IsRequired();

         builder.Property(ti => ti.PaymentStatus)
            .HasMaxLength(30)
            .IsRequired();
    }
}
