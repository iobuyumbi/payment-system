using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Solidaridad.Core.Entities.Loans;

namespace Solidaridad.DataAccess.Persistence.Configurations;

public class ApplicationStatusLogConfiguration : IEntityTypeConfiguration<ApplicationStatusLog>
{
    public void Configure(EntityTypeBuilder<ApplicationStatusLog> builder)
    {

        builder.Property(ti => ti.StatusId)
             .IsRequired();

        builder.Property(ti => ti.Comments)
            .HasMaxLength(500)
            .IsRequired(false);
    }
}
