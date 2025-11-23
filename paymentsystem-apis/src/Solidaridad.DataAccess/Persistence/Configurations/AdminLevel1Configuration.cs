using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Solidaridad.Core.Entities;

namespace Solidaridad.DataAccess.Persistence.Configurations;

public class AdminLevel1Configuration : IEntityTypeConfiguration<AdminLevel1>
{
    public void Configure(EntityTypeBuilder<AdminLevel1> builder)
    {
        builder.Property(ti => ti.CountyName)
          .HasMaxLength(126)
          .IsRequired();

        builder.Property(ti => ti.CountyCode)
            .HasMaxLength(15)
            .IsRequired(false);

        builder.Property(ti => ti.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(ti => ti.CountryId)
            .IsRequired();
    }
}
