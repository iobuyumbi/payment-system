using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Solidaridad.Application.Models.AdminLevels;
using Solidaridad.Application.Models.Country;
using Solidaridad.Application.Models;
using Solidaridad.Application.Services;
using System.Collections.ObjectModel;
using Solidaridad.Application.Models.DocumentType;

namespace Solidaridad.API.Controllers;
[Authorize]
public class DocumentTypeController : ApiController
{
    #region DI
    private readonly IDocumentTypeService _documentTypeService;
    public DocumentTypeController(IDocumentTypeService documentTypeService)
    {
        _documentTypeService = documentTypeService;
    }
    #endregion

    #region Methods
    [HttpGet]
    public async Task<IActionResult> GetAll(bool? isActive)
    {
        var types = await _documentTypeService.GetAllAsync();
      

        return Ok(ApiResult<IEnumerable<DocumentTypeResponseModel>>.Success(types));
    }
   
    #endregion
}
