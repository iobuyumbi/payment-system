using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Solidaridad.Core.Entities;

namespace Solidaridad.DataAccess.Persistence.Configurations;

public class ActivityLogConfiguration : IEntityTypeConfiguration<ActivityLog>
{
    public void Configure(EntityTypeBuilder<ActivityLog> builder)
    {
        builder.Property(ti => ti.Title)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(ti => ti.Description)
           .HasMaxLength(4000);

        builder.Property(ti => ti.Link)
           .HasMaxLength(4000);
    }
}
