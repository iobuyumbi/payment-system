using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Solidaridad.Core.Entities.Loans;

namespace Solidaridad.DataAccess.Persistence.Configurations
{
    public class LoanApplicationConfiguration : IEntityTypeConfiguration<LoanApplication>
    {
        public void Configure(EntityTypeBuilder<LoanApplication> builder)
        {

            builder.Property(ti => ti.WitnessFullName)
                .HasMaxLength(126)
               .IsRequired(false);

            builder.Property(ti => ti.WitnessNationalId)
                .HasMaxLength(30)
               .IsRequired(false);

            builder.Property(ti => ti.WitnessPhoneNo)
                .HasMaxLength(20)
               .IsRequired(false);

            builder.Property(ti => ti.WitnessRelation)
               .HasMaxLength(50)
               .IsRequired(false);

            builder.Property(ti => ti.DateOfWitness)
               .IsRequired();

            builder.Property(ti => ti.EnumeratorFullName)
                .HasMaxLength(126)
               .IsRequired(false);

            builder.Property(ti => ti.LoanBatchId)
              .IsRequired();

            builder.Property(ti => ti.FarmerId)
             .IsRequired();


            builder.Property(ti => ti.LoanNumber)
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(ti => ti.RemainingBalance)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(ti => ti.PrincipalAmount)
               .HasPrecision(18, 2)
               .IsRequired();

            builder.Property(ti => ti.InterestAmount)
                .HasPrecision(18, 2)
                .IsRequired();
        }
    }
}
