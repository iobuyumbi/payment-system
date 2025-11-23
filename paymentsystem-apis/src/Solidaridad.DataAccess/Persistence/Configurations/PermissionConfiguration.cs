using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Solidaridad.Core.Entities;

namespace Solidaridad.DataAccess.Persistence.Configurations;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.Property(ti => ti.PermissionName)
               .HasMaxLength(50)
               .IsRequired();

        builder.Property(ti => ti.ModuleId)
              .IsRequired();

        builder.Property(ti => ti.Description)
            .HasMaxLength(500)
            .IsRequired();

    }
}
