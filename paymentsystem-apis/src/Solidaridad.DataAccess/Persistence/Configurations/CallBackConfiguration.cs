using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Solidaridad.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solidaridad.DataAccess.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CallBackConfiguration : IEntityTypeConfiguration<CallBackRecords>
{
    public void Configure(EntityTypeBuilder<CallBackRecords> builder)
    {
        builder.Property(ti => ti.Organization)
            .IsRequired();

        builder.Property(ti => ti.Amount)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(ti => ti.Currency)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(ti => ti.PaymentType)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(ti => ti.Metadata) 
            .IsRequired();

        builder.Property(ti => ti.Description)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(ti => ti.State)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(ti => ti.LastError)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(ti => ti.RejectedReason)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(ti => ti.RejectedBy)
            .IsRequired(false);

        builder.Property(ti => ti.RejectedTime)
            .IsRequired(false);

        builder.Property(ti => ti.CancelledReason)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(ti => ti.CancelledBy)
            .IsRequired(false);

        builder.Property(ti => ti.CancelledTime)
            .IsRequired(false);

        builder.Property(ti => ti.Created)
            .IsRequired();

        builder.Property(ti => ti.Author)
            .IsRequired();

        builder.Property(ti => ti.Modified)
            .IsRequired();

        builder.Property(ti => ti.UpdatedBy)
            .IsRequired(false);

        builder.Property(ti => ti.StartDate)
            .IsRequired();

        builder.Property(ti => ti.PhoneNos)
            .IsRequired();
    }
}
