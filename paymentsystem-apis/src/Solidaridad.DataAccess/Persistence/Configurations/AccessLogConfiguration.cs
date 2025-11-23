using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Solidaridad.Core.Entities;

namespace Solidaridad.DataAccess.Persistence.Configurations;

public class AccessLogConfiguration : IEntityTypeConfiguration<AccessLog>
{
    public void Configure(EntityTypeBuilder<AccessLog> builder)
    {
        builder.Property(ti => ti.UserName)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(ti => ti.IpAddress)
           .HasMaxLength(50);

        builder.Property(ti => ti.UserAgent)
           .HasMaxLength(1024);

        builder.Property(ti => ti.Message)
          .HasMaxLength(4000);
    }
}
