using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Solidaridad.Core.Entities.Loans;

namespace Solidaridad.DataAccess.Persistence.Configurations;

public class MasterLoanTermConfiguration : IEntityTypeConfiguration<MasterLoanTerm>
{
    public void Configure(EntityTypeBuilder<MasterLoanTerm> builder)
    {
        builder.Property(ti => ti.DescriptiveName)
            .HasMaxLength(150)
                .IsRequired();

        builder.Property(ti => ti.InterestRateType)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(ti => ti.InterestRate)
           .IsRequired();

        builder.Property(ti => ti.InterestApplication)
             .HasMaxLength(50)
           .IsRequired();

        builder.Property(ti => ti.Tenure)
           .IsRequired();

           builder.Property(ti => ti.GracePeriod)
           .IsRequired();
    }
}
