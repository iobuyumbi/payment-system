using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Solidaridad.Core.Entities.Locations;

namespace Solidaridad.DataAccess.Persistence.Configurations;

public class LocationProfileConfiguration : IEntityTypeConfiguration<LocationProfile>
{
    public void Configure(EntityTypeBuilder<LocationProfile> builder)
    {
        builder.Property(lp => lp.LogoUrl)
            .HasMaxLength(2048);

        builder.Property(lp => lp.AddressLine1)
            .HasMaxLength(256);

        builder.Property(lp => lp.AddressLine2)
            .HasMaxLength(256);

        builder.Property(lp => lp.City)
            .HasMaxLength(100);

        builder.Property(lp => lp.State)
            .HasMaxLength(100);

        builder.Property(lp => lp.ZipCode)
            .HasMaxLength(20);

        builder.Property(lp => lp.SupportEmail)
            .HasMaxLength(150);

        builder.Property(lp => lp.PhoneNumber)
            .HasMaxLength(20);
        builder.Property(lp => lp.AlternateNumber)
         .HasMaxLength(20);
        builder.Property(lp => lp.Website)
         .HasMaxLength(256);
    }
}
