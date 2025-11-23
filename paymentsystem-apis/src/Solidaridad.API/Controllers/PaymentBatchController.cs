using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.PaymentBatch;
using Solidaridad.Application.Models.PaymentBatch.Transitions;
using Solidaridad.Application.Services;
using Solidaridad.Core.Entities.Base;
using Solidaridad.Core.Entities.Pagination;

namespace Solidaridad.API.Controllers;

[Authorize]
public class PaymentBatchController : ApiController
{
    #region DI
    private readonly IPaymentBatchService _batchService;
    public PaymentBatchController(IPaymentBatchService batchService)
    {
        _batchService = batchService;
    }
    #endregion

    #region Methods
    [HttpPost]
    [Route("Search")]
    public async Task<IActionResult> Search(PaymentSearchParams searchParams)
    {
        searchParams.CountryId = CountryId;
        var batches = await _batchService.GetAllAsync(searchParams);

        int totalRecords = batches.PaymentBatchResponseModel.Count();
        Page pageInfo = new Page
        {
            PageNumber = searchParams.PageNumber,
            Size = searchParams.PageSize,
            TotalElements = totalRecords,
            TotalPages = totalRecords / searchParams.PageSize
        };
        var pagedData = new PagedData<PaymentBatchesResponseModel>
        {
            Page = pageInfo,
            Result = batches
        };

        return Ok(new ApiResponseModel<PagedData<PaymentBatchesResponseModel>>
        {
            Success = true,
            Message = "success",
            Data = pagedData
        });
    }

    [HttpGet]
    [Route("single/{id:guid}")]
    public async Task<IActionResult> GetSingle(Guid id)
    {
        var countryId = CountryId;
        var single = await _batchService.GetSingle(id, countryId);

        return Ok(new ApiResponseModel<PaymentBatchResponseModel>
        {
            Success = true,
            Message = "success",
            Data = single
        });
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add(CreatePaymentBatchModel model)
    {
        model.CountryId = Convert.ToString(CountryId);
        return Ok(ApiResult<CreatePaymentBatchResponseModel>.Success(
            await _batchService.CreateAsync(model)));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, UpdatePaymentBatchModel model)
    {
        return Ok(ApiResult<UpdatePaymentBatchResponseModel>.Success(
            await _batchService.UpdateAsync(id, model)));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        return Ok(ApiResult<BaseResponseModel>.Success(await _batchService.DeleteAsync(id)));
    }

    [HttpGet("history/{id:guid}")]
    public async Task<IActionResult> GetHistory(Guid id)
    {
        return Ok(ApiResult<object>.Success(
            await _batchService.GetPaymentBatchHistory(id)));
    }

    [HttpPut("update-stage/{id:guid}")]
    public async Task<IActionResult> UpdateStageAsync(Guid id, UpdateStageModel model)
    {
        return Ok(ApiResult<UpdatePaymentBatchResponseModel>.Success(
            await _batchService.UpdateStage(id, model)));
    }

    [HttpGet("send-email/{id:guid}")]
    public async Task<IActionResult> SendEmailAsync(Guid id)
    {
        return Ok(ApiResult<bool>.Success(
            await _batchService.SendEmail(id)));
    }


    [HttpGet]
    [Route("stats")]
    public async Task<IActionResult> GetStats()
    {
        var stats = await _batchService.GetStats(CountryId);

        return Ok(new ApiResponseModel<PaymentStatsResponseModel>
        {
            Success = true,
            Message = "success",
            Data = stats
        });
    }

    [HttpPut("process-manually/{id:guid}")]
    public async Task<IActionResult> ProcessManuallyAsync(Guid id)
    {
        return Ok(ApiResult<UpdatePaymentBatchResponseModel>.Success(
            await _batchService.ProcessManually(id)));
    }
    #endregion

    [HttpGet("{id}/action-context")]
    public async Task<IActionResult> GetContext(Guid id)
    {
        var batch = await _batchService.GetSingle(id, CountryId);
        var result = _batchService.GetActionContext(batch);

        return Ok(ApiResult<TransitionResponse>.Success(result));
    }

    [HttpPost("{id}/transition")]
    public async Task<IActionResult> PerformTransition(Guid id, [FromBody] TransitionRequest request)
    {
        var batch = await _batchService.GetSingle(id, CountryId);

        if (_batchService.TryTransition(batch, request, out var error))
        {
            await _batchService.UpdateStage(id, new UpdateStageModel
            {
                Action = request.Action,
                Remarks = request.Remarks,
            });
            return Ok(ApiResult<BaseResponseModel>.Success(new BaseResponseModel { Id = id }));
        }

        return BadRequest(error);
    }
}
