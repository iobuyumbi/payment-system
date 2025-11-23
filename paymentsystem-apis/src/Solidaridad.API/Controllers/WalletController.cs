using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Solidaridad.Application.Models.County;
using Solidaridad.Application.Models;
using Solidaridad.Application.Services;
using Solidaridad.Application.Services.Impl;
using Solidaridad.Core.Entities.Base;
using Solidaridad.Core.Entities.Pagination;
using Solidaridad.Application.Models.Wallet;

namespace Solidaridad.API.Controllers;

[Authorize]
public class WalletController : ApiController
{
    #region DI
    private readonly IWalletService _walletService;
    public WalletController(IWalletService walletService)
    {
        _walletService = walletService;
    }
    #endregion
    #region Methods
    [HttpPost]
    [Route("Search")]
    public async Task<IActionResult> GetAll(WalletSearchParams searchParams)
    {
        var wallets = await _walletService.GetAllAsync(searchParams);
        int totalRecords = wallets.Count();
        Page pageInfo = new Page
        {
            PageNumber = searchParams.PageNumber,
            Size = searchParams.PageSize,
            TotalElements = totalRecords,
            TotalPages = totalRecords / searchParams.PageSize
        };
        var pagedData = new PagedData<List<WalletResponseModel>>
        {
            Page = pageInfo,
            Result = wallets.ToList()
        };

        return Ok(new ApiResponseModel<PagedData<List<WalletResponseModel>>>
        {
            Success = true,
            Message = "success",
            Data = pagedData
        });


    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateWalletModel model)
    {
        return Ok(ApiResult<CreateWalletResponseModel>.Success(
            await _walletService.CreateAsync(model)));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(UpdateWalletModel model, Guid id)
    {
        return Ok(ApiResult<UpdateWalletResponseModel>.Success(
            await _walletService.UpdateAsync(id,model)));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        return Ok(ApiResult<BaseResponseModel>.Success(await _walletService.DeleteAsync(id)));
    }
    #endregion
}
