namespace Solidaridad.Application.Models.ExcelImport;

public class ExcelImportModel
{
    public int Id { get; set; }
    public Guid OrgId { get; set; }
    public DateTime ImportedDateTime { get; set; }
    public string Filename { get; set; }
    public string Module { get; set; }
    public int? ExcelImportStatusID { get; set; }
    public string StatusRemark { get; set; }
    public int? UserId { get; set; }
    public DateTime? EndDateTime { get; set; }
}

public class ExcelImportVM
{
    public int ImportId { get; set; }
    public DateTime ImportedDateTime { get; set; }
    public DateTime? EndDateTime { get; set; }
    public string Filename { get; set; }
    public string ImportDate { get; set; }
    public string ImportTime { get; set; }
    public string Status { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
    public string Email { get; set; }
    public string Enterprise { get; set; }
    public int RowDataCount { get; set; }
}
