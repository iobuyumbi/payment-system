using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Solidaridad.Core.Entities;

namespace Solidaridad.DataAccess.Persistence.Configurations;

public class ApiRequestLogConfiguration : IEntityTypeConfiguration<ApiRequestLog>
{
    public void Configure(EntityTypeBuilder<ApiRequestLog> builder)
    {
        builder.Property(ti => ti.ApiName)
             .HasMaxLength(150)
             .IsRequired();

        builder.Property(ti => ti.HttpMethod)
            .HasMaxLength(50)
            .IsRequired();
    }
}
