using Solidaridad.Core.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Solidaridad.Core.Entities.Loans;

[Table("LoanItems")]

public class LoanItem : BaseEntity
{
    public string ItemName { get; set; }

    public string Description { get; set; }

    public Guid CategoryId { get; set; }

    public decimal Quantity { get; set; }

    public decimal? UnitPrice { get; set; }

    public Guid MasterLoanItemId { get; set; }

    public Guid LoanApplicationId { get; set; }

    public int UnitId { get; set; }

    public ItemUnit Unit { get; set; }

    public MasterLoanItem MasterLoanItem { get; set; }

    public LoanApplication LoanApplication { get; set; }
}

[Table("LoanItemImportStaging")]
public class LoanItemImportStaging : BaseEntity
{
    [StringLength(250)] public string ItemName { get; set; }

    public string Description { get; set; }

    public Guid? CategoryId { get; set; }

    public decimal Quantity { get; set; }

    public decimal? UnitPrice { get; set; }

    public Guid? MasterLoanItemId { get; set; }

    public Guid? LoanApplicationId { get; set; }

    public int? UnitId { get; set; }

    public Guid ExcelImportId { get; set; }

    public int StatusId { get; set; } = 0;

    public int RowNumber { get; set; }

    public string ValidationErrors { get; set; }
}
