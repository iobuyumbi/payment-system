namespace Solidaridad.Application.Models.ExcelImport;

public class ExcelImportResponseModel : BaseResponseModel
{
    public string Filename { get; set; }

    public DateTime ImportedDateTime { get; set; }

    public DateTime? EndDateTime { get; set; }

    public string Module { get; set; }

    public int? ExcelImportStatusID { get; set; }

    public string StatusRemarks { get; set; }

    public Guid OrgId { get; set; }

    public Guid CountryId { get; set; }

    public string BlobFolder { get; set; }
}

