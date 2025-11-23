namespace Solidaridad.Application.Models.ExcelExport;

public class DeductibleExportModel
{
    public int StatusId {  get; set; }
    public Guid BatchId { get; set; }
    public bool? IsFarmerValid { get; set; }
    public Guid? CountryId { get; set; }
}
