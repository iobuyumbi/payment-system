using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Solidaridad.Core.Entities;
using Solidaridad.Core.Entities.Loans;

namespace Solidaridad.DataAccess.Persistence.Configurations;

public class LoanItemConfiguration : IEntityTypeConfiguration<LoanItem>
{
    public void Configure(EntityTypeBuilder<LoanItem> builder)
    {
        builder.Property(ti => ti.ItemName)
           .HasMaxLength(126)
           .IsRequired();

        builder.Property(ti => ti.Description)
            .HasMaxLength(500)
            .IsRequired(false);

     
    }
}
