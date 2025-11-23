using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Solidaridad.Core.Entities.Email;

namespace Solidaridad.DataAccess.Persistence.Configurations;

public class EmailTemplateVariableConfiguration : IEntityTypeConfiguration<EmailTemplateVariable>
{
    public void Configure(EntityTypeBuilder<EmailTemplateVariable> builder)
    {
        builder.Property(etv => etv.Name)
            .IsRequired()
            .HasColumnName("Name");

        builder.Property(etv => etv.DefaultValue)
            .HasColumnName("DefaultValue");
    }
}

