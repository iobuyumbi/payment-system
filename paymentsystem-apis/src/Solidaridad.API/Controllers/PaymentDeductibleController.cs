using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.ExcelImport;
using Solidaridad.Application.Models.PaymentDeductible;
using Solidaridad.Application.Services;
using Solidaridad.Core.Entities.Base;
using Solidaridad.Core.Entities.Pagination;

namespace Solidaridad.API.Controllers;

[Authorize]
public class PaymentDeductibleController : ApiController
{
    #region DI
    private readonly IPaymentDeductibleService _paymentDeductibleService;
    public PaymentDeductibleController(IPaymentDeductibleService paymentDeductibleService)
    {
        _paymentDeductibleService = paymentDeductibleService;
    }
    #endregion

    #region methods
    [HttpPost]
    [Route("Search")]
    public async Task<IActionResult> Search(DeductibleSearchParams searchParams)
    {
        var batches = await _paymentDeductibleService.GetAllAsync(searchParams);

        int totalRecords = batches.Count();
        Page pageInfo = new Page
        {
            PageNumber = searchParams.PageNumber,
            Size = searchParams.PageSize,
            TotalElements = totalRecords,
            TotalPages = totalRecords / searchParams.PageSize
        };
        var pagedData = new PagedData<List<PaymentDeductibleResponseModel>>
        {
            Page = pageInfo,
            Result = batches.ToList()
        };

        return Ok(new ApiResponseModel<PagedData<List<PaymentDeductibleResponseModel>>>
        {
            Success = true,
            Message = "success",
            Data = pagedData
        });
    }
    
    [HttpGet]
    [Route("report")]
    public async Task<IActionResult> GetReport()
    {
        var data = await _paymentDeductibleService.GetAllForReportAsync();

        return Ok(new ApiResponseModel<IEnumerable<PaymentRequestDeductibleModel>>
        {
            Success = true,
            Message = "success",
            Data = data
        });
    }

    [HttpGet]
    [Route("single/{id:guid}")]
    public async Task<IActionResult> GetSingle(Guid id)
    {
        var single = await _paymentDeductibleService.GetByIdAsync(id);

        return Ok(new ApiResponseModel<PaymentDeductibleResponseModel>
        {
            Success = true,
            Message = "success",
            Data = single
        });
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add(CreatePaymentDeductibleModel model)
    {
        return Ok(ApiResult<CreatePaymentDeductibleResponseModel>.Success(
            await _paymentDeductibleService.CreateAsync(model)));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, UpdatePaymentDeductibleModel model)
    {
        return Ok(ApiResult<UpdatePaymentDeductibleResponseModel>.Success(
            await _paymentDeductibleService.UpdateAsync(id, model)));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        return Ok(ApiResult<BaseResponseModel>.Success(await _paymentDeductibleService.DeleteAsync(id)));
    }

    [HttpGet("GetPaymentSummary")]
    public async Task<IActionResult> GetSummary()
    {
        return Ok(ApiResult<IEnumerable<PaymentSummaryResponseModel>>.Success(await _paymentDeductibleService.GetAllPaymentsData()));
    }
  
    #endregion
}
