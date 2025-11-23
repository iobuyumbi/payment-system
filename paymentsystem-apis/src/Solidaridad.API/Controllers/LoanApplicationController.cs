using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.ApplicationStatusLog;
using Solidaridad.Application.Models.EmiSchedule;
using Solidaridad.Application.Models.LoanApplication;
using Solidaridad.Application.Models.PaymentBatch;
using Solidaridad.Application.Services;
using Solidaridad.Core.Entities;
using Solidaridad.Core.Entities.Base;
using Solidaridad.Core.Entities.Loans;
using Solidaridad.Core.Entities.Pagination;

namespace Solidaridad.API.Controllers;

[Authorize]
public class LoanApplicationController : ApiController
{
    #region DI
    private readonly ILoanApplicationService _loanApplicationService;
    //private readonly IPaymentService _paymentService;
    private readonly ILoanRepaymentService _loanRepaymentService;

    public LoanApplicationController(ILoanApplicationService loanApplicationService, ILoanRepaymentService loanRepaymentService)
    {
        _loanApplicationService = loanApplicationService;
        _loanRepaymentService = loanRepaymentService;
    }
    #endregion

    #region Methods
    [HttpPost]
    [Route("Search")]
    public async Task<IActionResult> GetAll(LoanApplicationSearchParams searchParams)
    {
        searchParams.CountryId = CountryId;
        var applications = await _loanApplicationService.GetAllAsync(searchParams);
        if (applications.Count() > 0)
        {
            int totalRecords = applications.Count();
            Page pageInfo = new Page
            {
                PageNumber = searchParams.PageNumber,
                Size = searchParams.PageSize,
                TotalElements = totalRecords,
                TotalPages = totalRecords / searchParams.PageSize
            };
            var pagedData = new PagedData<List<LoanApplicationResponseModel>>
            {
                Page = pageInfo,
                Result = applications.OrderByDescending(c => c.CreatedOn).ToList()
            };

            return Ok(new ApiResponseModel<PagedData<List<LoanApplicationResponseModel>>>
            {
                Success = true,
                Message = "success",
                Data = pagedData
            });
        }

        return Ok(new ApiResponseModel<PagedData<List<LoanApplicationResponseModel>>>
        {
            Success = false,
            Message = "error",
            Data = null
        });
    }

    [HttpGet("GetDocument/{id:guid}")]
    public async Task<IActionResult> GetApplicationDocuments(Guid id)
    {
        return Ok(ApiResult<LoanApplicationResponseModel>.Success(
            await _loanApplicationService.GetApplicationDocuments(id)));
    }

    [HttpPost]
    public async Task<IActionResult> ImportAsync(ImportLoanApplicationModel importLoanApplication)
    {
        return Ok(ApiResult<ImportLoanApplicationResponseModel>.Success(
            await _loanApplicationService.ImportAsync(importLoanApplication)));
    }

    [HttpPost("add")]
    public async Task<IActionResult> CreateAsync(CreateLoanApplicationModel createLoanApplication)
    {
        var result = await _loanApplicationService.CreateAsync(createLoanApplication);

        if(result.Message == "existing application")
        {

            return Ok(new ApiResponseModel<PagedData<List<LoanApplicationResponseModel>>>
            {
                Success = false,
                Message = "Farmer already has an existing loan application in current product.",
                Data = null
            });
        }
        return Ok(ApiResult<CreateLoanApplicationResponseModel>.Success(
            result));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, UpdateLoanApplicationModel loanApplicationModel)
    {
        return Ok(ApiResult<UpdateLoanApplicationResponseModel>.Success(
            await _loanApplicationService.UpdateAsync(id, loanApplicationModel)));
    }

    [HttpPut("StatusUpdate/{statusId:guid}")]
    public async Task<IActionResult> UpdateStatusAsync(Guid statusId, IEnumerable<ApplicationStatusEditModel> loanApplicationModel)
    {
        return Ok(ApiResult<ApplicationStatusEditResponseModel>.Success(
            await _loanApplicationService.UpdateStatusAsync(statusId, loanApplicationModel)));
    }

    [HttpPut("update-stage/{id:guid}")]
    public async Task<IActionResult> UpdateStageAsync(Guid id, UpdateStageModel model)
    {
        return Ok(ApiResult<UpdateLoanApplicationResponseModel>.Success(
            await _loanApplicationService.UpdateStage(id, model)));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        return Ok(ApiResult<BaseResponseModel>.Success(await _loanApplicationService.DeleteAsync(id)));
    }

    [HttpGet("farmer/{farmerId:guid}")]
    public async Task<IActionResult> GetFarmerLoanApps(Guid farmerId)
    {
        var countryId = CountryId;

        return Ok(ApiResult<IEnumerable<LoanApplicationResponseModel>>.Success(
            await _loanApplicationService.GetFarmerLoanApps(farmerId, (Guid)countryId)));
    }

    [HttpGet("emi-schedule/{loanApplicationId:guid}")]
    public async Task<IActionResult> GetEmiSchedule(Guid loanApplicationId)
    {
        var schedule = await _loanApplicationService.GetEmiSchedule(loanApplicationId);
        var application = await _loanApplicationService.GetSingleAsync(loanApplicationId);

        return Ok(ApiResult<object>.Success(new
        {
            Application = application,
            Schedule = schedule
        }));
    }

    [HttpPut("update-emi-schedule/{loanApplicationId:guid}")]
    public async Task<IActionResult> UpdateEmiSchedule(Guid loanApplicationId, ReceivedPaymentModel receivedPayment)
    {
        var schedule = await _loanApplicationService.UpdateEMISchedule(
            receivedPayment.PaymentReceived,
            receivedPayment.PaymentDate ?? DateTime.UtcNow,
            loanApplicationId);


        return Ok(ApiResult<List<EMISchedule>>.Success(schedule));
    }

    [HttpGet("StatusLog/{id:guid}")]
    public async Task<IActionResult> GetStatusHistory(Guid id)
    {
        return Ok(ApiResult<IEnumerable<ApplicationStatusLogResponseModel>>.Success(
            await _loanApplicationService.GetStatusHistory(id)));
    }

    [HttpGet("calculate-interest/{loanApplicationId:guid}")]
    public async Task<IActionResult> CalculateLoanInterestAsync(Guid loanApplicationId)
    {
        var result = await _loanRepaymentService.SimulateTransaction(loanApplicationId);

        return Ok(ApiResult<List<LoanTransaction>>.Success(result));
    }
    
    [HttpGet("import-summary/{batchId:guid}")]
    public async Task<IActionResult> GetImportSummary(Guid batchId)
    {
        return Ok(ApiResult<LoanApplicationTrackModel>.Success(
            await _loanApplicationService.GetImportSummary(batchId)));
    }

    [HttpGet("import-history/{batchId:guid}")]
    public async Task<IActionResult> GetImportHistory(Guid batchId)
    {
        return Ok(ApiResult<List<LoanAppImportModel>>.Success(
            await _loanApplicationService.GetImportHistory(batchId)));
    }


    [HttpGet("Schedule/{id:guid}")]
    public async Task<IActionResult> GetSchedule(Guid id)
    {
        return Ok(ApiResult<IEnumerable<EMISchedule>>.Success(
            await _loanApplicationService.GetLatestEMISchedule(id)));
    }



    #endregion
}

public class ReceivedPaymentModel
{
    public decimal PaymentReceived { get; set; }
    public DateTime? PaymentDate { get; set; }
}

