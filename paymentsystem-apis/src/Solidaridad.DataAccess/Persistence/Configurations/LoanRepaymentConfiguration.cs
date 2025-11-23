using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Solidaridad.Core.Entities.Loans;

namespace Solidaridad.DataAccess.Persistence.Configurations;

public class LoanRepaymentConfiguration : IEntityTypeConfiguration<LoanRepayment>
{
    public void Configure(EntityTypeBuilder<LoanRepayment> builder)
    {
        builder.Property(ti => ti.LoanApplicationId).IsRequired();
        builder.Property(ti => ti.AmountPaid).IsRequired().HasPrecision(18, 2);
        builder.Property(ti => ti.PaymentMode).HasMaxLength(50);
        builder.Property(ti => ti.ReferenceNumber).HasMaxLength(100);
    }
}
