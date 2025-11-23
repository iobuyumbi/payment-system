using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.LoanProcessingFee;
using Solidaridad.Application.Services;

namespace Solidaridad.API.Controllers;

[Authorize]
public class MasterProcessingFeeController : ApiController
{
    #region DI
    private readonly ILoanProcessingFeeService _loanProcessingFeeService;
    private readonly IMapper _mapper;
    public MasterProcessingFeeController(ILoanProcessingFeeService loanProcessingFeeService, IMapper mapper)
    {
        _loanProcessingFeeService = loanProcessingFeeService;
        _mapper = mapper;
    }
    #endregion

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var loanProcessingFees = await _loanProcessingFeeService.GetAllAsync();

        return Ok(new ApiResponseModel<IEnumerable<LoanProcessingFeeResponseModel>>
        {
            Success = true,
            Message = "success",
            Data = loanProcessingFees
        });
    }

    [HttpPost]
    public async Task<IActionResult> Add(CreateLoanProcessingFeeModel createLoanProcessingFeeModel)
    {
        return Ok(ApiResult<CreateLoanProcessingFeeResponseModel>.Success(
            await _loanProcessingFeeService.CreateAsync(createLoanProcessingFeeModel)));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, UpdateLoanProcessingFeeModel updateLoanProcessingFeeModel)
    {
        return Ok(ApiResult<UpdateLoanProcessingFeeResponseModel>.Success(
            await _loanProcessingFeeService.UpdateAsync(id, updateLoanProcessingFeeModel)));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        return Ok(ApiResult<BaseResponseModel>.Success(await _loanProcessingFeeService.DeleteAsync(id)));
    }

}
