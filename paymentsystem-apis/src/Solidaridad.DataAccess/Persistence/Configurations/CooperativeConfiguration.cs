using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Solidaridad.Core.Entities;

namespace Solidaridad.DataAccess.Persistence.Configurations;

public class CooperativeConfiguration : IEntityTypeConfiguration<Cooperative>
{
    public void Configure(EntityTypeBuilder<Cooperative> builder)
    {

        builder.Property(ti => ti.Name)
             .HasMaxLength(126)
             .IsRequired();

        builder.Property(ti => ti.CountryId)
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(ti => ti.Description).HasMaxLength(500)
            .IsRequired(false);
    }
}
