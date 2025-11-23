using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Solidaridad.Core.Entities;
using Solidaridad.Core.Entities.Loans;

namespace Solidaridad.DataAccess.Persistence.Configurations;

public class MasterLoanItemConfiguration : IEntityTypeConfiguration<MasterLoanItem>
{
    public void Configure(EntityTypeBuilder<MasterLoanItem> builder)
    {
        builder.Property(ti => ti.ItemName)
           .HasMaxLength(126)
           .IsRequired();

        builder.Property(ti => ti.Description)
            .HasMaxLength(500)
            .IsRequired(false);

     
    }
}
