using LinqToDB.Mapping;


namespace Solidaridad.Core.Entities.Excels.Import
{
    [Table("ExcelImport")]
    public class ExcelImport_na
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
}
