using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.LoanTerm;
using Solidaridad.Application.Services;
using Solidaridad.Core.Entities.Base;

namespace Solidaridad.API.Controllers;

[Authorize]
public class MasterLoanTermController : ApiController
{
    private readonly IMasterLoanTermService _loanTermService;
    public MasterLoanTermController(IMasterLoanTermService loanTermService)
    {
        _loanTermService = loanTermService;
    }

    [HttpPost]
    [Route("Search")]
    public async Task<IActionResult> Search(SearchParams searchParams)
    {
        searchParams.CountryId = CountryId;
        var loanTerms = await _loanTermService.GetAllAsync(searchParams);

        return Ok(new ApiResponseModel<IEnumerable<MasterLoanTermResponseModel>>
        {
            Success = true,
            Message = "success",
            Data = loanTerms
        });
    }

    [HttpPost]
    public async Task<IActionResult> Add(CreateMasterLoanTermModel createLoanTermModel)
    {
        createLoanTermModel.CountryId = (Guid)CountryId;
        return Ok(ApiResult<CreateMasterLoanTermResponseModel>.Success(
            await _loanTermService.CreateAsync(createLoanTermModel)));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, UpdateMasterLoanTermModel updateFarmerModel)
    {
        updateFarmerModel.CountryId = (Guid)CountryId;
        return Ok(ApiResult<UpdateMasterLoanTermResponseModel>.Success(
            await _loanTermService.UpdateAsync(id, updateFarmerModel)));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        return Ok(ApiResult<BaseResponseModel>.Success(await _loanTermService.DeleteAsync(id)));
    }
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        return Ok(ApiResult<MasterLoanTermResponseModel>.Success(await _loanTermService.GetByIdAsync(id, CountryId.Value)));
    }

}
