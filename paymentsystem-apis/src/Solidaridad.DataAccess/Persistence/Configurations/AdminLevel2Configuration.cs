using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Solidaridad.Core.Entities;

namespace Solidaridad.DataAccess.Persistence.Configurations;

public class AdminLevel2Configuration : IEntityTypeConfiguration<AdminLevel2>
{
    public void Configure(EntityTypeBuilder<AdminLevel2> builder)
    {
        builder.Property(ti => ti.SubCountyName)
           .HasMaxLength(126)
           .IsRequired();

        builder.Property(ti => ti.SubCountyCode)
            .HasMaxLength(15)
            .IsRequired(false);

        builder.Property(ti => ti.IsActive)
            .IsRequired(true)
            .HasDefaultValue(true);

        builder.Property(ti => ti.CountyId)
            .IsRequired();
       

    }
}
