using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Solidaridad.Core.Entities;

namespace Solidaridad.DataAccess.Persistence.Configurations;

public class LogEntryConfiguration : IEntityTypeConfiguration<LogEntry>
{
    public void Configure(EntityTypeBuilder<LogEntry> builder)
    {
        builder.Property(ti => ti.Level)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(ti => ti.Message)
            .IsRequired(false);

        builder.Property(ti => ti.Exception)
            .IsRequired(false);
    }
}
