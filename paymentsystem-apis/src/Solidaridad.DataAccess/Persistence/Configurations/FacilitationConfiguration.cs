using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Solidaridad.Core.Entities.Payments;

namespace Solidaridad.DataAccess.Persistence.Configurations;

public class FacilitationConfiguration : IEntityTypeConfiguration<PaymentRequestFacilitation>
{
    public void Configure(EntityTypeBuilder<PaymentRequestFacilitation> builder)
    {
        builder.Property(ti => ti.FullName).IsRequired();
        builder.Property(ti => ti.PhoneNo).IsRequired();
        builder.Property(ti => ti.NetDisbursementAmount).IsRequired();
        builder.Property(ti => ti.Comments).IsRequired(false);
    }
}

