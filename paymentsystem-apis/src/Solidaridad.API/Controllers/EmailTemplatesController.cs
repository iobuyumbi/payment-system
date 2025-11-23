using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.Email;
using Solidaridad.Application.Services;

namespace Solidaridad.API.Controllers;

[Authorize]
public class EmailTemplatesController : ApiController
{
    #region DI

    private readonly IEmailTemplateService _emailTemplateService;

    public EmailTemplatesController(IEmailTemplateService emailTemplateService)
    {
        _emailTemplateService = emailTemplateService;
    }

    #endregion

    #region Methods

    [HttpGet]
    public async Task<IActionResult> GetAll(string keyword)
    {
        return Ok(ApiResult<IEnumerable<EmailTemplateResponseModel>>.Success(await _emailTemplateService.GetAllAsync(keyword)));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        return Ok(ApiResult<EmailTemplateResponseModel>.Success(await _emailTemplateService.GetByIdAsync(id)));
    }

    [HttpGet("{name}")]
    public async Task<IActionResult> GetByName(string name)
    {
        return Ok(ApiResult<EmailTemplateResponseModel>.Success(await _emailTemplateService.GetByNameAsync(name)));
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateEmailTemplateModel createEmailTemplateModel)
    {
        return Ok(ApiResult<CreateEmailTemplateResponseModel>.Success(
            await _emailTemplateService.CreateAsync(createEmailTemplateModel)));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, UpdateEmailTemplateModel updateEmailTemplateModel)
    {
        return Ok(ApiResult<UpdateEmailTemplateResponseModel>.Success(
            await _emailTemplateService.UpdateAsync(id, updateEmailTemplateModel)));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        return Ok(ApiResult<BaseResponseModel>.Success(await _emailTemplateService.DeleteAsync(id)));
    }

    #endregion
}
