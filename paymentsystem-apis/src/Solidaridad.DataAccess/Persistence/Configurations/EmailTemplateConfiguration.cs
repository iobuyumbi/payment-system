using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Solidaridad.Core.Entities.Email;

namespace Solidaridad.DataAccess.Persistence.Configurations;

public class EmailTemplateConfiguration : IEntityTypeConfiguration<EmailTemplate>
{
    public void Configure(EntityTypeBuilder<EmailTemplate> builder)
    {
        builder.Property(ti => ti.Name)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(ti => ti.Subject)
           .HasMaxLength(500)
           .IsRequired();

        builder.Property(ti => ti.Body)
               .IsRequired();
    }
}
