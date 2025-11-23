namespace Solidaridad.Application.Models.LoanApplication;

public class ImportLoanApplicationModel
{
    public string SystemId { get; set; }

    public bool IsRegistered { get; set; }

    public string FarmSize { get; set; }
    public Guid Status { get; set; }

    public string WitnessFullName { get; set; }

    public string WitnessNationalId { get; set; }

    public string WitnessPhoneNo { get; set; }

    public string WitnessRelation { get; set; }

    public string DateOfWitness { get; set; }

    public string EnumeratorFullName { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public Guid? LoanBatchId { get; set; }

    public decimal? GrandTotal { get; set; }

    public virtual List<LoanAppItemImportModel> LoanItems { get; set; }

    public string Country { get; internal set; }
    public string OfficerId { get; set; }

}

public class ImportLoanApplicationResponseModel : BaseResponseModel
{
    public bool Success { get; set; } = true;
    public List<string> Errors { get; set; } = new List<string>();
}

public class ImportLoanItemsModel
{
    public string SystemId { get; set; }
    public string ItemName { get; set; }
    public decimal PricePerUnit { get; set; }
    public decimal Quantity { get; set; }
    public string Unit { get; set; }
}
