using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Solidaridad.Core.Entities;

namespace Solidaridad.DataAccess.Persistence.Configurations;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.Property(al => al.Id).IsRequired().HasMaxLength(36);

        builder.Property(al => al.ChangedBy).IsRequired().HasMaxLength(100);

        builder.Property(al => al.ChangedOn).IsRequired();

        builder.Property(al => al.OldValue).IsRequired(false);
        
        builder.Property(al => al.NewValue).IsRequired(false);

        builder.Property(al => al.ChangeType).IsRequired().HasMaxLength(50);
    }
}
