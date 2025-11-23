using NPOI.SS.Formula.Functions;
using Solidaridad.Application.Models.ApplicationStatusLog;
using Solidaridad.Application.Models.AttachmentUpload;
using Solidaridad.Application.Models.Farmer;
using Solidaridad.Application.Models.LoanAttachmentModel;
using Solidaridad.Core.Entities.Loans;

namespace Solidaridad.Application.Models.LoanApplication;

public class LoanApplicationResponseModel : BaseResponseModel
{
    public Guid FarmerId { get; set; }

    public bool IsRegistered { get; set; }

    public string FarmSize { get; set; }

    public float TotalValue { get; set; }

    public string WitnessFullName { get; set; }

    public string WitnessNationalId { get; set; }

    public Guid Status { get; set; }

    public string WitnessPhoneNo { get; set; }

    public string WitnessRelation { get; set; }

    public DateTime DateOfWitness { get; set; }

    public string EnumeratorFullName { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public Guid? LoanBatchId { get; set; }

    public FarmerResponseModel Farmer { get; set; }

    public LoanBatchResponseModel LoanBatch { get; set; }

    public List<LoanAppItemImportModel> LoanItems { get; set; }

    public List<AttachmentResponseModel> AttachmentFiles { get; set; }

    public ApplicationStatusLogResponseModel ApplicationStatusLog { get; set; }

    public string Moderator { get; set; }

    public string ModeratorRole { get; set; }

    public decimal PrincipalAmount { get; set; }

    public decimal InterestAmount { get; set; }

    public decimal RemainingBalance { get; set; }

    public bool InUse { get; set; }

    public string LoanNumber { get; set; }

    public decimal FeeApplied { get; set; }

    public string StatusText { get; set; }
    public decimal  PrincipalDue { get; set; }
    public decimal PrincipalReceived { get; set; }
    public decimal  TotalInterest { get; set; }
    public decimal InterestDue  { get; set; }
    public decimal  InterestReceived { get; set; }
    public decimal InterestArrears { get; set; }
    public decimal PrincipalArrears { get; set; }
    public decimal TotalArrears { get; set; }

    public decimal TotalExpected { get; set; }
    public decimal ArrearsRate { get; set; }
    public DateTime DisbursedOn { get; set; }
    public Guid OfficerId { get; set; }

    public string CurrentUserName { get; set; }
}

public class FarmerLoanAppsResponseModel
{
    public decimal Amount { get; set; }

    public decimal InterestRate { get; set; }

    public DateTime? EffectiveDate { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public Guid LoanApplicationId { get; set; }

    public int GracePeriod { get; internal set; }

    public int TenureMonths { get; internal set; }

    public string LoanNumber { get; internal set; }
}

public class LoanApplicationTrackModel
{
    public LoanBatchResponseModel LoanBatch { get; set; }

    public List<LoanApplicationImportStaging> ImportedLoanApplications { get; set; }

    public int ImportedRowCount { get; set; } = 0;
    public int SuccessRowCount { get; set; } = 0;
    public int FailedRowCount { get; set; } = 0;
}

public class LoanAppImportModel
{
    //public LoanAppImportModel()
    //{

    //}

    public Guid ExcelImportId { get; set; }

    public string FileName { get; set; }

    public DateTime ImportedDateTime { get; set; }

    public bool IsFailedBatch { get; set; }

    public LoanBatchResponseModel LoanBatch { get; internal set; }

    public List<LoanApplicationStagingModel> Applications { get; set; } = new();

    public List<LoanItemImportStaging> LoanItems { get; set; } = new();
}

public class LoanApplicationStagingModel
{
    public Guid Id { get; set; }

    public int RowNumber { get; set; }

    public int StatusId { get; set; }

    public string ValidationErrors { get; set; }

    public Guid ExcelImportId { get; set; }

    public Guid LoanBatchId { get; set; }

    public Guid? FarmerId { get; set; }

    public string WitnessFullName { get; set; }

    public string WitnessNationalId { get; set; }

    public string WitnessPhoneNo { get; set; }

    public string WitnessRelation { get; set; }

    public DateTime DateOfWitness { get; set; }

    public string EnumeratorFullName { get; set; }

    //public Guid LoanBatchId { get; set; }

    public decimal PrincipalAmount { get; set; }

    public decimal EffectivePrincipal { get; set; }

    public decimal AccruedInterest { get; set; }

    public decimal InterestAmount { get; set; }

    public decimal InterestRate { get; set; }

    public string LoanNumber { get; set; }

    public decimal RemainingBalance { get; set; }

    public decimal FeeApplied { get; set; }

    //public Guid ExcelImportId { get; set; }

    //public int RowNumber { get; set; }

    public string ValidationStatus { get; set; }

    //public string ValidationErrors { get; set; }
}

