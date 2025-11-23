using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Solidaridad.Core.Entities.Loans;

namespace Solidaridad.DataAccess.Persistence.Configurations;

public class ItemCategoryConfiguration : IEntityTypeConfiguration<ItemCategory>
{
    public void Configure(EntityTypeBuilder<ItemCategory> builder)
    {
        builder.Property(ti => ti.Name)
          .HasMaxLength(126)
          .IsRequired();

        builder.Property(ti => ti.Description)
            .HasMaxLength(500)
            .IsRequired(false);
    }
}
