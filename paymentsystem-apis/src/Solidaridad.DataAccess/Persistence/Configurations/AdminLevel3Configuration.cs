using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Solidaridad.Core.Entities;

namespace Solidaridad.DataAccess.Persistence.Configurations;

public class AdminLevel3Configuration : IEntityTypeConfiguration<AdminLevel3>
{
    public void Configure(EntityTypeBuilder<AdminLevel3> builder)
    {
        builder.Property(ti => ti.WardName)
          .HasMaxLength(126)
          .IsRequired();

        builder.Property(ti => ti.WardCode)
            .HasMaxLength(15)
            .IsRequired(false);

        builder.Property(ti => ti.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(ti => ti.SubCountyId)
            .IsRequired();
    }
}
