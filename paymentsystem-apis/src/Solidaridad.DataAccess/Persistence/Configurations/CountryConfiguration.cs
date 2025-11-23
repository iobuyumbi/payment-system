using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Solidaridad.Core.Entities;

namespace Solidaridad.DataAccess.Persistence.Configurations;

public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.Property(ti => ti.CountryName)
           .HasMaxLength(126)
           .IsRequired();

        builder.Property(ti => ti.Code)
            .HasMaxLength(5)
            .IsRequired();

        builder.Property(ti => ti.IsActive)
            .IsRequired();
        builder.Property(ti => ti.CurrencyName)
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(ti => ti.CurrencyPrefix)
           .HasMaxLength(5)
           .IsRequired();
        builder.Property(ti => ti.CurrencySuffix)
         .HasMaxLength(5)
         .IsRequired();

    }

   }

