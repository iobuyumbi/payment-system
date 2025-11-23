using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Solidaridad.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solidaridad.DataAccess.Persistence.Configurations;

public class BatchAttachmentMappingConfiguration : IEntityTypeConfiguration<BatchAttachmentMapping>
{
    public void Configure(EntityTypeBuilder<BatchAttachmentMapping> builder)
    {

        builder.Property(ti => ti.LoanBatchId)
            .IsRequired();

        builder.Property(ti => ti.AttachmentId)
            .IsRequired();
    }
}
