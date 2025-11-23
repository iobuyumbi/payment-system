using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Solidaridad.Core.Entities;

namespace Solidaridad.DataAccess.Persistence.Configurations;

public class PaymentBatchProjectMappingConfiguration : IEntityTypeConfiguration<PaymentBatchProjectMapping>
{
    public void Configure(EntityTypeBuilder<PaymentBatchProjectMapping> builder)
    {
        

        builder.Property(ti => ti.PaymentBatchId)
           .IsRequired();

        builder.Property(ti => ti.ProjectId)
           .IsRequired();

     
    }
}
