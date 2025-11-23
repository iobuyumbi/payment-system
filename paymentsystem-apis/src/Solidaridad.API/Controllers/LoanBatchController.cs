using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Solidaridad.Application.Models.Cooperative;
using Solidaridad.Application.Models;
using Solidaridad.Core.Entities.Base;
using Solidaridad.Core.Entities.Pagination;
using Solidaridad.Application.Services;
using Solidaridad.Application.Models.LoanApplication;
using Solidaridad.Application.Models.LoanBatch;
using MailKit.Search;
using Solidaridad.Application.Models.PaymentBatch;
using Solidaridad.Application.Models.AttachmentUpload;
using Solidaridad.DataAccess.Migrations;

namespace Solidaridad.API.Controllers;

[Authorize]
public class LoanBatchController : ApiController
{
    #region DI
    private readonly ILoanBatchService _loanBatchService;
    public LoanBatchController(ILoanBatchService loanBatchService)
    {
        _loanBatchService = loanBatchService;
    }
    #endregion

    #region Loan batch
    [HttpPost]
    [Route("Search")]
    public async Task<IActionResult> Search(LoanBatchSearchParams loanBatchSearchParams)
    {
        loanBatchSearchParams.CountryId = CountryId;
        var cooperatives = await _loanBatchService.GetAllAsync(loanBatchSearchParams);

        int totalRecords = cooperatives.LoanBatches.Count();
        Page pageInfo = new Page
        {
            PageNumber = loanBatchSearchParams.PageNumber,
            Size = loanBatchSearchParams.PageSize,
            TotalElements = totalRecords,
            TotalPages = totalRecords / loanBatchSearchParams.PageSize
        };
        var pagedData = new PagedData<LoanBatchesResponseModel>
        {
            Page = pageInfo,
            Result = cooperatives
        };

        return Ok(new ApiResponseModel<PagedData<LoanBatchesResponseModel>>
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
        var single = await _loanBatchService.GetSingle(id);

        return Ok(new ApiResponseModel<LoanBatchResponseModel>
        {
            Success = true,
            Message = "success",
            Data = single
        });
    }

    [HttpPost]
    [Route("SearchByProjects")]
    public async Task<IActionResult> SearchByProjects(List<string> projectIds)
    {
        var list = _loanBatchService.GetByProjectIds(projectIds);

        return Ok(new ApiResponseModel<object>
        {
            Success = true,
            Message = "success",
            Data = list
        });
    }

    [HttpPost]
    public async Task<IActionResult> Add(CreateLoanBatchModel createLoanBatchModel)
    {
        createLoanBatchModel.CountryId = CountryId;

        return Ok(ApiResult<CreateLoanBatchResponseModel>.Success(
            await _loanBatchService.CreateAsync(createLoanBatchModel)));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, UpdateLoanBatchModel updateLoanBatchModel)
    {
        return Ok(ApiResult<UpdateLoanBatchResponseModel>.Success(
            await _loanBatchService.UpdateAsync(id, updateLoanBatchModel)));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        return Ok(ApiResult<BaseResponseModel>.Success(await _loanBatchService.DeleteAsync(id)));
    }

    [HttpGet]
    [Route("valid")]
    public async Task<IActionResult> GetValidLoanBatches()
    {
        var list = await _loanBatchService.GetValidLoanBatches((Guid)CountryId);

        return Ok(new ApiResponseModel<List<LoanBatchResponseModel>>
        {
            Success = true,
            Message = "success",
            Data = list.OrderBy(x => x.Name).ToList()
        });
    }
    #endregion

    #region Loan batch item
    [HttpPost("CreateLoanBatchItem")]
    public async Task<IActionResult> CreateLoanBatchItem(CreateLoanBatchItemModel createLoanBatchItemMapModel)
    {
        return Ok(ApiResult<CreateLoanBatchItemResponseModel>.Success(
            await _loanBatchService.CreateLoanBatchItem(createLoanBatchItemMapModel)));
    }
    [HttpPut("UpdateLoanBatchItem/{id:guid}")]
    public async Task<IActionResult> UpdateLoanBatchItemAsync(Guid id, UpdateLoanBatchItemModel updateLoanBatchItemModel)
    {
        return Ok(ApiResult<UpdateLoanBatchItemResponseModel>.Success(
            await _loanBatchService.UpdateLoanBatchItemAsync(id, updateLoanBatchItemModel)));
    }

    [HttpGet("GetBatchItems/{loanBatchId:guid}")]
    public async Task<IActionResult> GetBatchItems(Guid loanBatchId)
    {
        return Ok(ApiResult<IEnumerable<LoanBatchItemResponseModel>>.Success(await _loanBatchService.GetBatchItems(loanBatchId)));
    }


    [HttpDelete("DeleteBatchItems/{id:guid}")]
    public async Task<IActionResult> DeleteBatchItemAsync(Guid id)
    {
        return Ok(ApiResult<BaseResponseModel>.Success(await _loanBatchService.DeleteBatchItemAsync(id)));
    }
    #endregion

    [HttpPut("update-stage/{id:guid}/{excelImportId:guid}")]
    public async Task<IActionResult> UpdateStageAsync(Guid id, Guid excelImportId, UpdateStageModel model)
    {
        return Ok(ApiResult<UpdateLoanBatchResponseModel>.Success(
            await _loanBatchService.UpdateStage(id, excelImportId, model)));
    }

    [HttpGet("GetBatchFiles/{loanBatchId:guid}")]
    public async Task<IActionResult> GetBatchFiles(Guid loanBatchId)
    {
        return Ok(ApiResult<IEnumerable<AttachmentResponseModel>>.Success(await _loanBatchService.GetBatchDocuments(loanBatchId)));
    }



}
