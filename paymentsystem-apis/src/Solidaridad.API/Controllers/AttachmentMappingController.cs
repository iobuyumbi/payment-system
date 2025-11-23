using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.LoanAttachmentModel;
using Solidaridad.Application.Services;
using Solidaridad.Core.Entities.Base;
using Solidaridad.Core.Entities.Pagination;

namespace Solidaridad.API.Controllers;

[Authorize]
public class AttachmentMappingController : ApiController
{
    #region DI
    private readonly IAttachmentMappingService _attachmentService;
    public AttachmentMappingController(IAttachmentMappingService attachmentService)
    {
        _attachmentService = attachmentService;
    }
    #endregion
    [HttpPost]
    [Route("Search")]
    public async Task<IActionResult> Search(AttachmentSearchParams attachmentSearchParams)
    {
        var attachments = await _attachmentService.GetAllAsync(attachmentSearchParams);

        int totalRecords = attachments.Count();
        Page pageInfo = new Page
        {
            PageNumber = attachmentSearchParams.PageNumber,
            Size = attachmentSearchParams.PageSize,
            TotalElements = totalRecords,
            TotalPages = totalRecords / attachmentSearchParams.PageSize
        };
        var pagedData = new PagedData<List<MappingResponseModel>>
        {
            Page = pageInfo,
            Result = attachments.ToList()
        };

        return Ok(new ApiResponseModel<PagedData<List<MappingResponseModel>>>
        {
            Success = true,
            Message = "success",
            Data = pagedData
        });
    }


    [HttpPost("add")]
    public async Task<IActionResult> Add(CreateAttachmentModel createAttachmentModel)
    {
        return Ok(ApiResult<CreateAttachmentResponseModel>.Success(
            await _attachmentService.CreateAsync(createAttachmentModel)));
    }

   

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, UpdateAttachmentModel updateAttachmentModel)
    {
        return Ok(ApiResult<UpdateAttachmentResponseModel>.Success(
            await _attachmentService.UpdateAsync(id, updateAttachmentModel)));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        return Ok(ApiResult<BaseResponseModel>.Success(await _attachmentService.DeleteAsync(id)));
    }






}
