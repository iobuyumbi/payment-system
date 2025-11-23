using Solidaridad.Core.Common;

namespace Solidaridad.Core.Entities.Payments;

public class PaymentImportSummary: BaseEntity
{
     public Guid ExcelImportId { get; set; }
    
    public int TotalRowsInExcel { get; set; }
    
    public int PassedRows { get; set; }
    
    public int FailedRows { get; set; }
}
