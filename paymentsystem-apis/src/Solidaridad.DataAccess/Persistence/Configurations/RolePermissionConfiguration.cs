using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Solidaridad.Core.Entities;

namespace Solidaridad.DataAccess.Persistence.Configurations;

public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.Property(ti => ti.RoleId)
               .HasMaxLength(36)
               .IsRequired();

        builder.Property(ti => ti.PermissionId)
              .IsRequired();

        builder.Property(ti => ti.IsDeleted)
              .HasDefaultValue(false)
              .IsRequired();
    }
}
