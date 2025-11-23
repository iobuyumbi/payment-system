using Solidaridad.Core.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Solidaridad.Core.Entities;

[Table("ExcelImport")]   

public class ExcelImport: BaseEntity
{
    public DateTime ImportedDateTime { get; set; }

    public string Filename { get; set; }
    
    public string BlobFolder { get; set; }
    
    public string BlobContainer { get; set; }
    
    public string Module { get; set; }
    
    public int? ExcelImportStatusID { get; set; }
    
    public Guid? PaymentBatchId { get; set; }

    public string StatusRemark { get; set; }
    
    public Guid? UserId { get; set; }
    
    public DateTime? EndDateTime { get; set; }

    public Guid CountryId { get; set; }
}
