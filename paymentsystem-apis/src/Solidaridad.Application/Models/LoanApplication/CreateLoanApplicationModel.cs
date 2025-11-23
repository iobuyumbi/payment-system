using Solidaridad.Core.Entities.Loans;

namespace Solidaridad.Application.Models.LoanApplication;

public class CreateLoanApplicationModel
{
    public SelectItemModel Farmer { get; set; }

    public bool IsRegistered { get; set; }

    public string LostReason { get; set; }

    public string WitnessFullName { get; set; }

    public string WitnessNationalId { get; set; }

    public string WitnessPhoneNo { get; set; }

    public string WitnessRelation { get; set; }
    public Guid Status { get; set; }

    public DateTime DateOfWitness { get; set; }

    public string EnumeratorFullName { get; set; }

    public Guid LoanBatchId { get; set; }
    
    public List<Guid> AttachmentIds { get; set; }

    public virtual List<LoanAppItemImportModel> LoanItems { get; set; }
    public decimal PrincipalAmount { get; set; }
    public string OfficerId { get; set; }
}

public class CreateLoanApplicationResponseModel : BaseResponseModel { 
public string Message { get; set; } = "Loan application created successfully";
}
