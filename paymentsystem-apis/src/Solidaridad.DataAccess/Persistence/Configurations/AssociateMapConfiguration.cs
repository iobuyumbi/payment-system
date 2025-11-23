using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Solidaridad.Core.Entities.Payments;

namespace Solidaridad.DataAccess.Persistence.Configurations;

public class AssociateMapConfiguration : IEntityTypeConfiguration<AssociateMap>
{
    public void Configure(EntityTypeBuilder<AssociateMap> builder)
    {
        builder.Property(ti => ti.FarmerId).IsRequired();
        builder.Property(ti => ti.PaymentBatchId).IsRequired();

    }
}
