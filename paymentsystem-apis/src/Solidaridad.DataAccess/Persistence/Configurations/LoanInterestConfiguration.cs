using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Solidaridad.Core.Entities.Loans;

namespace Solidaridad.DataAccess.Persistence.Configurations;

public class LoanInterestConfiguration : IEntityTypeConfiguration<LoanInterest>
{
    public void Configure(EntityTypeBuilder<LoanInterest> builder)
    {
        builder.Property(ti => ti.MonthlyPayment).IsRequired().HasPrecision(18,2);
        builder.Property(ti => ti.RemainingPrincipal).IsRequired().HasPrecision(18,2);
        builder.Property(ti => ti.CalculationMonth).IsRequired();
        builder.Property(ti => ti.LoanApplicationId).IsRequired();
    }
}
