using Solidaridad.Application.Models.Country;
using Solidaridad.Application.Models.LoanApplication;
using Solidaridad.Application.Models.PaymentDeductible;
using Solidaridad.Application.Models.Project;
using Solidaridad.Core.Entities;
using Solidaridad.Core.Enums;

namespace Solidaridad.Application.Models.PaymentBatch;

public class PaymentBatchResponseModel : BaseResponseModel
{
    public PaymentBatchResponseModel()
    {
        PaymentStats = new PaymentStats { BeneficiaryCount = 0, TotalAmount = 0 };
    }
    public string BatchName { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public int PaymentModule { get; set; }

    public CountryResponseModel Country { get; set; }

    public IEnumerable<LoanBatchResponseModel> LoanBatches { get; set; }

    public IEnumerable<ProjectResponseModel> Projects { get; set; }

    public Guid StatusId { get; set; }

    public int ExcelImportId { get; set; }

    public MasterPaymentApprovalStage Status { get; set; }

    public PaymentStats PaymentStats { get; set; }

    public ProjectResponseModel Project { get; set; }

    public Guid? ProjectId { get; set; }

    public int SuccessRowCount { get; internal set; } = 0;

    public int FailedRowCount { get; internal set; } = 0;

    public string NationalId { get; set; }

    public string UpdatedBy { get; set; }

    public string CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public BatchStatus BatchStatus { get; internal set; }
}

public class PaymentBatchStatsModel
{
    public int TotalBatches { get; set; }
}
public class PaymentBatchesResponseModel
{
    public IEnumerable<PaymentBatchResponseModel> PaymentBatchResponseModel { get; set; }
    public PaymentBatchStatsModel PaymentBatchStats { get; set; }
}
