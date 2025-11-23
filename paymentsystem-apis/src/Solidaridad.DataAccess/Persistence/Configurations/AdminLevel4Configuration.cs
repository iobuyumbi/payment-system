using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Solidaridad.Core.Entities;

namespace Solidaridad.DataAccess.Persistence.Configurations;

public class AdminLevel4Configuration : IEntityTypeConfiguration<AdminLevel4>
{
    public void Configure(EntityTypeBuilder<AdminLevel4> builder)
    {
        builder.Property(ti => ti.VillageName)
           .HasMaxLength(126)
           .IsRequired();

        builder.Property(ti => ti.VillageCode)
            .HasMaxLength(15)
            .IsRequired(false);

        builder.Property(ti => ti.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(ti => ti.WardId).IsRequired();
       

    }
}
