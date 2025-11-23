namespace Solidaridad.Application.Models.ExcelImport;

public class UpdateExcelImportModel
{
    public Guid ExcelImportId { get; set; }

    public DateTime? EndDateTime { get; set; }

    public int? ExcelImportStatusID { get; set; }

    public string StatusRemarks { get; set; }
}

public class UpdateExcelImportResponseModel : BaseResponseModel { }

