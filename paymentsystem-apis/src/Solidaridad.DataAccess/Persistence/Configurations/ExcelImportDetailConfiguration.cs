using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Solidaridad.Core.Entities.Excel.Import;

namespace Solidaridad.DataAccess.Persistence.Configurations;

public class ExcelImportDetailConfiguration : IEntityTypeConfiguration<ExcelImportDetail>
{
    public void Configure(EntityTypeBuilder<ExcelImportDetail> builder)
    {
        builder.Property(ti => ti.TabName)
          .HasMaxLength(126)
          .IsRequired();

        builder.Property(ti => ti.Remarks)
            .HasMaxLength(4000);
    }
}
