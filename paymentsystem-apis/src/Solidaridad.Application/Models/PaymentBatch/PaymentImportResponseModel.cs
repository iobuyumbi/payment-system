namespace Solidaridad.Application.Models.PaymentBatch;

public class PaymentImportResponseModel
{
    public Guid ExcelImportId { get; set; }

    public int TotalRowsInExcel { get; set; }

    public int PassedRows { get; set; }

    public int FailedRows { get; set; }

    public DateTime ImportDate { get; set; }
}
