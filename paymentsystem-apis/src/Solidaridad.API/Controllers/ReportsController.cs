using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.ApiRequestLog;
using Solidaridad.Application.Models.LoanApplication;
using Solidaridad.Application.Models.Location;
using Solidaridad.Application.Models.PaymentBatch;
using Solidaridad.Application.Models.Reports;
using Solidaridad.Application.Services;
using Solidaridad.Core.Entities;
using Solidaridad.Core.Entities.Base;
using Solidaridad.Core.Entities.Pagination;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Solidaridad.API.Controllers;

[Authorize]
public class ReportsController : ApiController
{
    #region DI
    private readonly IReportService _reportService;
    private readonly IPaymentBatchService _batchService;
    private readonly IPaymentDeductibleService _paymentDeductibleService;

    public ReportsController(IReportService reportService, IPaymentBatchService batchService, IPaymentDeductibleService paymentDeductibleService)
    {
        _reportService = reportService;
        _batchService = batchService;
        _paymentDeductibleService = paymentDeductibleService;
    }
    #endregion

   #region Outdated
    [HttpGet("stats/{year}")]
    public async Task<ActionResult> GetStats(int year)
    {
        Guid countryId = (Guid)CountryId;
        var report = await _reportService.GetStats(year , countryId);

        return Ok(ApiResult<List<KeyMetricsModel>>.Success(report));
    }

    [HttpGet("job-execution-log")]
    public async Task<ActionResult> GetJobExecutionLog()
    {
        var report = await _reportService.GetJobExecutionLog();

        return Ok(ApiResult<List<JobExecutionLog>>.Success(report.OrderByDescending(c => c.StartTime).ToList()));
    }

    [HttpPost("paymentStats")]
    public async Task<IActionResult> Search(PaymentSearchParams searchParams)
    {
        var batches = await _batchService.GetAllAsync(searchParams);
        var list = batches.PaymentBatchResponseModel.ToList();

        foreach (var bat in list)
        {
            var status = await _paymentDeductibleService.GetPaymentBatchStats(bat.Id.Value);
            bat.PaymentStats = status;
        }
        return Ok(ApiResult<List<PaymentBatchResponseModel>>.Success(list));
    }

    [HttpGet("payments-chart")]
    public async Task<IActionResult> GetPayments()
    {
        var payments = new List<PaymentData>
        {
            new PaymentData { Month = "Feb", Amount = 30 },
            new PaymentData { Month = "Mar", Amount = 40 },
            new PaymentData { Month = "Apr", Amount = 40 },
            new PaymentData { Month = "May", Amount = 90 },
            new PaymentData { Month = "Jun", Amount = 90 },
            new PaymentData { Month = "Jul", Amount = 70 },
            new PaymentData { Month = "Aug", Amount = 70 }
        };

       return Ok(ApiResult<List<PaymentData>>.Success(payments));
    }


    #region PaymentBatchReports
    [HttpPost("GetPaymentConfirmationReport")]
    public async Task<IActionResult> GetPaymentConfirmationReport(SearchParams searchParams)
    {
        var batches = await _reportService.GetPaymentConfirmationReport(searchParams);
        int totalRecords = batches.PaymentDeductibles.Count();
        Page pageInfo = new Page
        {
            PageNumber = searchParams.PageNumber,
            Size = searchParams.PageSize,
            TotalElements = totalRecords,
            TotalPages = totalRecords / searchParams.PageSize
        };
        var pagedData = new PagedData<PaymentConfirmationResponseModel>
        {
            Page = pageInfo,
            Result = batches
        };

        return Ok(new ApiResponseModel<PagedData<PaymentConfirmationResponseModel>>
        {
            Success = true,
            Message = "success",
            Data = pagedData
        });
    }

    #endregion

    #region LoanAccountReports
    [HttpPost("GetLoanAccountReports")]
    public async Task<IActionResult> GetLoanAccountReports(SearchParams searchParams)
    {
        var applications = await _reportService.GetLoanAccountReports(searchParams);
        int totalRecords = applications.LoanApplications.Count();
        Page pageInfo = new Page
        {
            PageNumber = searchParams.PageNumber,
            Size = searchParams.PageSize,
            TotalElements = totalRecords,
            TotalPages = totalRecords / searchParams.PageSize
        };
        var pagedData = new PagedData<LoanAccountResponseModel>
        {
            Page = pageInfo,
            Result = applications
        };

        return Ok(new ApiResponseModel<PagedData<LoanAccountResponseModel>>
        {
            Success = true,
            Message = "success",
            Data = pagedData
        });
    }

    #endregion


    #region LoanBatchReports
    [HttpPost("GetLoanBatchReports")]
    public async Task<IActionResult> GetLoanBatchReports(SearchParams searchParams)
    {
        searchParams.CountryId = CountryId;
        var applications = await _reportService.GetLoanBatchReports(searchParams);
        int totalRecords = applications.LoanApplications.Count();
        Page pageInfo = new Page
        {
            PageNumber = searchParams.PageNumber,
            Size = searchParams.PageSize,
            TotalElements = totalRecords,
            TotalPages = totalRecords / searchParams.PageSize
        };
        var pagedData = new PagedData<LoanBatchReportResponseModel>
        {
            Page = pageInfo,
            Result = applications
        };

        return Ok(new ApiResponseModel<PagedData<LoanBatchReportResponseModel>>
        {
            Success = true,
            Message = "success",
            Data = pagedData
        });
    }

    [HttpGet("RepaymentMonthlyTrends")]
    public async Task<IActionResult> GetRepaymentTrends()
    {

       var result = await _reportService.GetRepaymentReports(CountryId);

        return Ok(ApiResult<IEnumerable<RepaymentReportsResponseModel>>.Success(result));
    }

    #endregion
#endregion




    [HttpPost("GetLoanApplicationReports")]
    public async Task<IActionResult> GetLoanApplicationReports(PortolioSearchParams portfolioParam)
    {
        portfolioParam.CountryId = CountryId;
        portfolioParam.CurrentUser = (Guid)CurrentUserId;
        var result = await _reportService.LoanApplicationReports(portfolioParam);

        return Ok(ApiResult<IEnumerable<LoanApplicationResponseModel>>.Success(result));
    }

    [HttpPost("GetLoanApplicationReportsPdf")]
    public async Task<IActionResult> GetLoanApplicationReportsPDF(PortolioSearchParams portfolioParam)
    {
        portfolioParam.CurrentUser = (Guid)CurrentUserId;
        var pdfBytes = await _reportService.GenerateLoanPortfolioPdfReport(portfolioParam);

        if (pdfBytes == null || pdfBytes.Length == 0)
            return NotFound("Failed to generate PDF.");

        var dateStamp = TimeZoneInfo.ConvertTimeFromUtc(
        DateTime.UtcNow,
        TimeZoneInfo.FindSystemTimeZoneById("E. Africa Standard Time")
        ).ToString("yyyy-MM-dd HH-mm");

        return File(pdfBytes, "application/pdf", $"LoanPortfolioReport_{dateStamp}.pdf");
    }

    [HttpPost("GetCountryLoanApplicationReports")]
    public async Task<IActionResult> CountryLoanApplicationReports(PortolioSearchParams portfolioParam)
    {
        
        portfolioParam.CurrentUser = (Guid)CurrentUserId;
        var result = await _reportService.CountryLoanApplicationReports(portfolioParam);

        return Ok(ApiResult<IEnumerable<LoanApplicationResponseModel>>.Success(result));
    }
    [HttpPost("GenerateCountryLoanPortfolioPdfReport")]
    public async Task<IActionResult> GenerateCountryLoanPortfolioPdfReport(PortolioSearchParams portfolioParam)
    {
        portfolioParam.CurrentUser = (Guid)CurrentUserId;
        var pdfBytes = await _reportService.GenerateCountryLoanPortfolioPdfReport(portfolioParam);

        if (pdfBytes == null || pdfBytes.Length == 0)
            return NotFound("Failed to generate PDF.");

        var dateStamp = TimeZoneInfo.ConvertTimeFromUtc(
        DateTime.UtcNow,
        TimeZoneInfo.FindSystemTimeZoneById("E. Africa Standard Time")
        ).ToString("yyyy-MM-dd HH-mm");

        return File(pdfBytes, "application/pdf", $"CountryLoanPortfolioReport_{dateStamp}.pdf");
    }


    [HttpPost("GlobalLoanPortfolioReports")]
    public async Task<IActionResult> GlobalLoanPortfolioReports(PortolioSearchParams portfolioParam)
    {

        portfolioParam.CurrentUser = (Guid)CurrentUserId;
        var result = await _reportService.GlobalLoanPortfolioReports(portfolioParam);

        return Ok(ApiResult<IEnumerable<GlobalLoanPortfolioReportResponseModel>>.Success(result));
    }

    [HttpPost("GenerateGlobalLoanPortfolioPdfReport")]
    public async Task<IActionResult> GenerateGlobalLoanPortfolioPdfReport(PortolioSearchParams portfolioParam)
    {
        portfolioParam.CurrentUser = (Guid)CurrentUserId;
        var pdfBytes = await _reportService.GenerateGlobalLoanPortfolioPdfReport(portfolioParam);

        if (pdfBytes == null || pdfBytes.Length == 0)
            return NotFound("Failed to generate PDF.");

        var dateStamp = TimeZoneInfo.ConvertTimeFromUtc(
        DateTime.UtcNow,
        TimeZoneInfo.FindSystemTimeZoneById("E. Africa Standard Time")
        ).ToString("yyyy-MM-dd HH-mm");

        return File(pdfBytes, "application/pdf", $"GlobalLoanPortfolioReport_{dateStamp}.pdf");
    }

    [HttpPost("GlobalLoanApplicationReports")]
    public async Task<IActionResult> GlobalLoanApplicationReports(PortolioSearchParams portfolioParam)
    {

        portfolioParam.CurrentUser = (Guid)CurrentUserId;
        var result = await _reportService.GlobalLoanApplicationReports(portfolioParam);

        return Ok(ApiResult<IEnumerable<GlobalLoanApplicationReportResponseModel>>.Success(result));
    }

    [HttpPost("GenerateGlobalLoanApplicationPdfReport")]
    public async Task<IActionResult> GenerateGlobalLoanApplicationPdfReport(PortolioSearchParams portfolioParam)
    {
        portfolioParam.CurrentUser = (Guid)CurrentUserId;
        var pdfBytes = await _reportService.GenerateGlobalLoanApplicationPdfReport(portfolioParam);

        if (pdfBytes == null || pdfBytes.Length == 0)
            return NotFound("Failed to generate PDF.");

        var dateStamp = TimeZoneInfo.ConvertTimeFromUtc(
        DateTime.UtcNow,
        TimeZoneInfo.FindSystemTimeZoneById("E. Africa Standard Time")
        ).ToString("yyyy-MM-dd HH-mm");

        return File(pdfBytes, "application/pdf", $"GlobalLoanPortfolioReport_{dateStamp}.pdf");
    }













    [HttpPost("GetDisbursedLoanReports")]
    public async Task<IActionResult> GetDisbursedLoanReports(DisbursedSearchParams portfolioParam)
    {
        portfolioParam.CountryId = CountryId;
        portfolioParam.CurrentUser = (Guid)CurrentUserId;
        var result = await _reportService.DisbursedLoanReports(portfolioParam);

        return Ok(ApiResult<IEnumerable<DisbursedLoanReportResponseModel>>.Success(result));
    }

    [HttpPost("GetDisbursedLoanReportsPdf")]
    public async Task<IActionResult> GetDisbursedLoanReportsPdf(DisbursedSearchParams portfolioParam)
    {
        portfolioParam.CurrentUser = (Guid)CurrentUserId;
        var pdfBytes = await _reportService.GenerateLoanDisbursementPdfReport(portfolioParam);

        if (pdfBytes == null || pdfBytes.Length == 0)
            return NotFound("Failed to generate PDF.");

        var dateStamp = TimeZoneInfo.ConvertTimeFromUtc(
        DateTime.UtcNow,
        TimeZoneInfo.FindSystemTimeZoneById("E. Africa Standard Time")
        ).ToString("yyyy-MM-dd HH-mm");

        return File(pdfBytes, "application/pdf", $"LoanPortfolioReport_{dateStamp}.pdf");
    }


    [HttpPost("GetPaymentReports")]
    public async Task<IActionResult> GetPaymentReports(PaymentReportSearchParams param)
    {
        param.CountryId = CountryId;
        param.CurrentUser = (Guid)CurrentUserId;
        var result = await _reportService.PaymentReports(param);
        if (!result.Any())
        {
            return Ok(new ApiResponseModel<PagedData<List<LocationResponseModel>>>
            {
                Success = false,
                Message = "No data found",
                Data = null
            });
        }
        return Ok(ApiResult<IEnumerable<PaymentReportResponseModel>>.Success(result));
    }

    [HttpPost("GetPaymentReportsPdf")]
    public async Task<IActionResult> GetPaymentReportsPdf(PaymentReportSearchParams param)
    {
        param.CurrentUser = (Guid)CurrentUserId;
        
        var pdfBytes = await _reportService.GeneratePaymentsPdfReport(param);

        if (pdfBytes == null || pdfBytes.Length == 0)
            return NotFound("Failed to generate PDF.");

        var dateStamp = TimeZoneInfo.ConvertTimeFromUtc(
        DateTime.UtcNow,
        TimeZoneInfo.FindSystemTimeZoneById("E. Africa Standard Time")
        ).ToString("yyyy-MM-dd HH-mm");

        return File(pdfBytes, "application/pdf", $"LoanPortfolioReport_{dateStamp}.pdf");
    }



  


}

public class PaymentData
{
    public string Month { get; set; }
    public decimal Amount { get; set; }
}
