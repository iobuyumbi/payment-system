using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Solidaridad.Core.Entities;
using Solidaridad.Core.Entities.Loans;

namespace Solidaridad.DataAccess.Persistence.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.Property(ti => ti.ProjectName)
           .HasMaxLength(126)
           .IsRequired();

        builder.Property(ti => ti.CountryId)
            .IsRequired();

        builder.Property(ti => ti.ProjectCode)
            .HasMaxLength(10)
            .IsRequired(false);
        builder.Property(ti => ti.Description)
           .HasMaxLength(500)
           .IsRequired(false);


    }
}
