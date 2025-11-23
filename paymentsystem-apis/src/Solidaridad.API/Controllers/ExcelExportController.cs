using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.ExcelExport;
using Solidaridad.Application.Models.ExcelImport;
using Solidaridad.Application.Services;
using Solidaridad.Core.Entities.Pagination;

namespace Solidaridad.API.Controllers;


[Authorize]
public class ExcelExportController : ApiController
{
    #region DI
    private IExcelExportService _excelExportService;

    public ExcelExportController(IExcelExportService excelExportService)
    {
        _excelExportService = excelExportService;
    }
    #endregion

    #region Public Methods

    [HttpPost]
    [Route("DeductiblePayments")]
    public async Task<IActionResult> GetDeductiblePayments(DeductibleExportModel model)
    {
        var payments = await _excelExportService.GetDeductiblePayments(model);

        int totalRecords = payments.Count();

        var pageInfo = new Page
        {
            PageNumber = 1,
            Size = totalRecords,
            TotalElements = totalRecords,
            TotalPages = totalRecords > 0 ? 1 : 0
        };

        var pagedData = new PagedData<List<PaymentRequestDeductibleModel>>
        {
            Page = pageInfo,
            Result = payments.ToList()
        };

        return Ok(new ApiResponseModel<PagedData<List<PaymentRequestDeductibleModel>>>
        {
            Success = true,
            Message = "Data retrieved successfully",
            Data = pagedData
        });
    }

    [HttpPost]
    [Route("PaymentHistory")]
    public async Task<IActionResult> GetPaymentHistory(DeductibleExportModel model)
    {
        var paymentDeductibles = await _excelExportService.GetDeductiblePayments(model);
        var paymentFacilitations = await _excelExportService.GetFacilitationPayments(model.BatchId);

        var payments = new List<PaymentModel>();

        // Map from deductibles
        payments.AddRange(paymentDeductibles.Select(d => new PaymentModel
        {
            Amount = d.FarmerPayableEarningsLc,
            FullName = d.Farmer.FullName,
            Mobile = d.Farmer.PaymentPhoneNumber,
            SystemId = d.Farmer.SystemId,
            IsFarmerVerified = d.Farmer.IsFarmerVerified,
            PaymentStatus = d.PaymentStatus,
            Remarks = d.Remarks,
            BeneficiaryId = d.BeneficiaryId,
            CarbonUnitsAccured = d.CarbonUnitsAccured,
            UnitCostEur = d.UnitCostEur,
            TotalUnitsEarningEur = d.TotalUnitsEarningEur,
            TotalUnitsEarningLc = d.TotalUnitsEarningLc,
            SolidaridadEarningsShare = d.SolidaridadEarningsShare,
            FarmerEarningsShareEur = d.FarmerEarningsShareEur,
            FarmerEarningsShareLc = d.FarmerEarningsShareLc,
            FarmerPayableEarningsLc = d.FarmerPayableEarningsLc,
            FarmerLoansDeductionsLc = d.FarmerLoansDeductionsLc,
            FarmerLoansBalanceLc = d.FarmerLoansBalanceLc,
            NationalId = d.NationalId,
            LoanAccountNo = d.LoanAccountNo
        }));

        // Map from facilitations
        payments.AddRange(paymentFacilitations.Select(f => new PaymentModel
        {
            Amount = f.NetDisbursementAmount,
            FullName = f.FullName,
            Mobile = f.PhoneNo,
            IsFarmerVerified = false,
            SystemId = "",
            PaymentStatus = f.PaymentStatus, 
            Remarks = f.Remarks ?? f.Comments,
            NationalId = f.NationalId
        }));

        int totalRecords = payments.Count();

        var pageInfo = new Page
        {
            PageNumber = 1,
            Size = totalRecords,
            TotalElements = totalRecords,
            TotalPages = totalRecords > 0 ? 1 : 0
        };

        var pagedData = new PagedData<List<PaymentModel>>
        {
            Page = pageInfo,
            Result = payments.ToList()
        };

        return Ok(new ApiResponseModel<PagedData<List<PaymentModel>>>
        {
            Success = true,
            Message = "Data retrieved successfully",
            Data = pagedData
        });
    }

    [HttpPost]
    [Route("GetAllDeductiblePayments")]
    public async Task<IActionResult> GetAllDeductiblePayments(DeductibleExportModel model)
    {
        var payments = await _excelExportService.GetAllDeductiblePayments();

        int totalRecords = payments.PaymentRequestDeductibleModels.Count();

        var pageInfo = new Page
        {
            PageNumber = 1,
            Size = totalRecords,
            TotalElements = totalRecords,
            TotalPages = totalRecords > 0 ? 1 : 0
        };

        var pagedData = new PagedData<PaymenReportResponseModel>
        {
            Page = pageInfo,
            Result = payments
        };

        return Ok(new ApiResponseModel<PagedData<PaymenReportResponseModel>>
        {
            Success = true,
            Message = "Data retrieved successfully",
            Data = pagedData
        });
    }
    #endregion
}


public class PaymentModel
{
    public string FullName { get; set; }

    public string Mobile { get; set; }

    public string SystemId { get; set; }

    public decimal Amount { get; set; }

    public bool? IsFarmerVerified { get; set; }

    public Guid? PaymentStatus { get; set; }

    public string Remarks { get; set; }

    public Guid PaymentBatchId { get; set; }

    public string BeneficiaryId { get; set; }

    public decimal CarbonUnitsAccured { get; set; }

    public decimal UnitCostEur { get; set; }

    public decimal TotalUnitsEarningEur { get; set; }

    public decimal TotalUnitsEarningLc { get; set; }

    public decimal SolidaridadEarningsShare { get; set; }

    public decimal FarmerEarningsShareEur { get; set; }

    public decimal FarmerEarningsShareLc { get; set; }

    public decimal FarmerPayableEarningsLc { get; set; }

    public decimal FarmerLoansDeductionsLc { get; set; }

    public decimal FarmerLoansBalanceLc { get; set; }

    public Guid? ExcelImportId { get; set; }

    public int StatusId { get; set; }
    public string NationalId { get; set; }
    public string LoanAccountNo { get; set; }
}
