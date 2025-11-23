using Solidaridad.Core.Common;

namespace Solidaridad.Core.Entities.Excel.Import;

public class ExcelImportDetail : BaseEntity
{
    public Guid ExcelImportId { get; set; }
    

    public string TabName { get; set; }
    
    public int RowNumber { get; set; }
    
    public bool IsSuccess { get; set; } = true;
    
    public string Remarks { get; set; }
}
