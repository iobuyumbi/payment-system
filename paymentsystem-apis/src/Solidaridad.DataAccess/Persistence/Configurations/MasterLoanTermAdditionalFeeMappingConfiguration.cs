using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Solidaridad.Core.Entities;
using Solidaridad.Core.Entities.Loans;

namespace Solidaridad.DataAccess.Persistence.Configurations;

internal class MasterLoanTermAdditionalFeeMappingConfiguration : IEntityTypeConfiguration<MasterLoanTermAdditionalFeeMapping>
{
    public void Configure(EntityTypeBuilder<MasterLoanTermAdditionalFeeMapping> builder)
    {
        builder.Property(ti => ti.LoanTermId)
            .IsRequired();

        builder.Property(ti => ti.AdditionalFeeId)
           .IsRequired();


    }

 
}  
