using LinqToDB.Mapping;


namespace Solidaridad.Core.Entities.Excels.Import
{
    [Table("ExcelImport")]
    public class ExcelImport
    {
        [PrimaryKey] public int Id { get; set; }
        [Column] public Guid OrgId { get; set; }
        [Column] public DateTime ImportedDateTime { get; set; }
        [Column] public string Filename { get; set; }
        [Column] public string BlobContainer { get; set; }
        [Column] public string BlobFolder { get; set; }
        [Column] public string Module { get; set; }
        [Column] public DateTime? EndDateTime { get; set; }
    }
    [Table("ExcelImportDetails")]
    public class ExcelImportDetails
    {
        [PrimaryKey] public long Id { get; set; }
        [Column] public int ExcelImportId { get; set; }
        [Column] public string TabName { get; set; }
        [Column] public long RowNumber { get; set; }
        [Column] public bool IsSuccess { get; set; }
        [Column] public string Remarks { get; set; }
    }
}
