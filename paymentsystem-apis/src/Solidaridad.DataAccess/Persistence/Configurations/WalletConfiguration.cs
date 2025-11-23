using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Solidaridad.Core.Entities;
using Solidaridad.Core.Entities.Wallets;

namespace Solidaridad.DataAccess.Persistence.Configurations;

public class WalletConfiguration : IEntityTypeConfiguration<Wallet>
{
    public void Configure(EntityTypeBuilder<Wallet> builder)
    {
        builder.Property(ti => ti.Balance)
          .IsRequired();

        builder.Property(ti => ti.OwnerId)
            .IsRequired();

        builder.Property(ti => ti.CreatedBy)
            .IsRequired();

        builder.Property(ti => ti.CreatedOn)
            .IsRequired();

        builder.Property(ti => ti.UpdatedBy)
           .IsRequired(false);
           

        builder.Property(ti => ti.UpdatedOn)
            .IsRequired(false);

    }
}
