using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Solidaridad.Core.Entities.Payments;

namespace Solidaridad.DataAccess.Persistence.Configurations;

public class PaymentRequestDeductibleConfiguration : IEntityTypeConfiguration<PaymentRequestDeductible>
{
    public void Configure(EntityTypeBuilder<PaymentRequestDeductible> builder)
    {
        builder.Property(ti => ti.SystemId)
          .HasMaxLength(126)
          .IsRequired();

        builder.Property(ti => ti.BeneficiaryId)
        .HasMaxLength(126)
        .IsRequired();

        builder.Property(ti => ti.CarbonUnitsAccured)
            .HasPrecision(18, 2);

        builder.Property(ti => ti.UnitCostEur)
           .HasPrecision(18, 2);

        builder.Property(ti => ti.TotalUnitsEarningEur)
           .HasPrecision(18, 2);

        builder.Property(ti => ti.TotalUnitsEarningLc)
           .HasPrecision(18, 2);

        builder.Property(ti => ti.SolidaridadEarningsShare)
           .HasPrecision(18, 2);

        builder.Property(ti => ti.FarmerEarningsShareEur)
           .HasPrecision(18, 2);

        builder.Property(ti => ti.FarmerEarningsShareLc)
           .HasPrecision(18, 2);

        builder.Property(ti => ti.FarmerPayableEarningsLc)
           .HasPrecision(18, 2);

        builder.Property(ti => ti.FarmerLoansDeductionsLc)
           .HasPrecision(18, 2);

        builder.Property(ti => ti.FarmerLoansBalanceLc)
          .HasPrecision(18, 2);

         builder.Property(ti => ti.AmountToTransfer)
          .HasPrecision(18, 2);

        builder.Property(ti => ti.CreatedBy)
            .HasMaxLength(36)
           .IsRequired(true);

        builder.Property(ti => ti.UpdatedBy)
           .HasMaxLength(36)
          .IsRequired(false);
    }
}
