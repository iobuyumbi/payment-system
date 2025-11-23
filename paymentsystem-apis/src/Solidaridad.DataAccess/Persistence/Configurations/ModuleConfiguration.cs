using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Solidaridad.Core.Entities;

namespace Solidaridad.DataAccess.Persistence.Configurations;

public class ModuleConfiguration : IEntityTypeConfiguration<Module>
{
    public void Configure(EntityTypeBuilder<Module> builder)
    {
        builder.Property(ti => ti.ModuleName)
               .HasMaxLength(50)
               .IsRequired();

        builder.Property(ti => ti.Description)
              .HasMaxLength(255)
              .IsRequired(false);
    }
}
