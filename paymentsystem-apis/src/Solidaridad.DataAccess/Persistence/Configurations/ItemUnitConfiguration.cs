using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Solidaridad.Core.Entities.Loans;

namespace Solidaridad.DataAccess.Persistence.Configurations;

public class ItemUnitConfiguration : IEntityTypeConfiguration<ItemUnit>
{
    public void Configure(EntityTypeBuilder<ItemUnit> builder)
    {
        builder.Property(ti => ti.Id)
            .IsRequired();

        builder.Property(ti => ti.Name)
             .HasMaxLength(50)
             .IsRequired();

        builder.Property(ti => ti.Abbreviation)
            .HasMaxLength(10)
            .IsRequired();
    }
}
