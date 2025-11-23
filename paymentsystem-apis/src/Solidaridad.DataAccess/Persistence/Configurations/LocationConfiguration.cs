using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Solidaridad.Core.Entities.Locations;

namespace Solidaridad.DataAccess.Persistence.Configurations;

public class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.Property(ti => ti.Name)
            .HasMaxLength(126)
            .IsRequired();

        // One-to-one relationship between Location and LocationProfile
       
    }
}
