using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.User;
using Solidaridad.Application.Services;
using Solidaridad.Core.Entities.Base;
using Solidaridad.Shared.Services;

namespace Solidaridad.API.Controllers;

//[Authorize]
public class UserController : ApiController
{
    #region DI
    private readonly IUserService _userService;
    private readonly IClaimService _claimService;

    public UserController(IUserService userService, IClaimService claimService)
    {
        _userService = userService;
        _claimService = claimService;
    }
    #endregion

    #region Methods
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetAllAsync();

        return Ok(ApiResult<IEnumerable<UserResponseModel>>.Success(users.ToList()));
    }

    [HttpGet("single")]
    public async Task<IActionResult> GetFirstAsync(Guid? id)
    {
        id = (id == Guid.Empty || id == null) ? new Guid(_claimService.GetUserId()) : id.Value;

        return Ok(ApiResult<UserResponseModel>.Success(
            await _userService.GetFirstAsync((Guid)id)));
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterAsync(CreateUserModel createUserModel)
    {
        return Ok(ApiResult<CreateUserResponseModel>.Success(await _userService.CreateAsync(createUserModel, forcePasswordChange: true)));
    }

    [HttpPost("authenticate")]
    [AllowAnonymous]
    public async Task<IActionResult> LoginAsync(LoginUserModel loginUserModel)
    {
        return Ok(ApiResult<LoginResponseModel>.Success(await _userService.LoginAsync(loginUserModel)));
    }

    [HttpPost("confirmEmail")]
    public async Task<IActionResult> ConfirmEmailAsync(ConfirmEmailModel confirmEmailModel)
    {
        return Ok(ApiResult<ConfirmEmailResponseModel>.Success(
            await _userService.ConfirmEmailAsync(confirmEmailModel)));
    }

    [HttpPut("changePassword")]
    public async Task<IActionResult> ChangePassword(ChangePasswordModel changePasswordModel)
    {
        Guid id = (Guid)CurrentUserId;
        return Ok(ApiResult<BaseResponseModel>.Success(
            await _userService.ChangePasswordAsync(id, changePasswordModel)));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, UpdateUserModel updateUserModel)
    {
        return Ok(ApiResult<UpdateUserResponseModel>.Success(await _userService.UpdateAsync(id, updateUserModel)));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        return Ok(ApiResult<BaseResponseModel>.Success(await _userService.DeleteAsync(id)));
    }

    [HttpGet("countries/{id}")]
    public async Task<IActionResult> GetUserCountries(string id)
    {
        return Ok(ApiResult<IEnumerable<SelectItemModel>>.Success(
            await _userService.GetUserCountries(id)));
    }

    [HttpGet("roles/{id}")]
    public async Task<IActionResult> GetRolesAsync(string id)
    {
        return Ok(ApiResult<IEnumerable<SelectItemModel>>.Success(
            await _userService.GetUserRoles(id)));
    }

    [HttpPost("ForgotPassword")]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordModel forgotPasswordModel)
    {
        return Ok(ApiResult<BaseResponseModel>.Success(
            await _userService.ForgotPasswordAsync(forgotPasswordModel)));
    }

    [HttpPost("ResetPassword")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword(ResetPasswordModel resetPasswordModel)
    {
        return Ok(ApiResult<BaseResponseModel>.Success(
           await _userService.ResetPasswordAsync(resetPasswordModel)));
    }

    [HttpGet("GetAllAuthorised")]
    
    public async Task<IActionResult> GetAllAuthorised(string permission)
    {
        AuthorisedUsersSearchParams authorisedUsersSearchParams = new AuthorisedUsersSearchParams
        {
            CountryId= CountryId,
            Permission = permission
        };
        var users = await _userService.GetAllAuthorisedAsync(authorisedUsersSearchParams);

        return Ok(ApiResult<IEnumerable<UserResponseModel>>.Success(users.ToList()));
    }

    [HttpGet("GetOfficerList")]
    public async Task<IActionResult> GetOfficerList()
    {

        var users = await _userService.GetOfficerList(CountryId);

        return Ok(ApiResult<IEnumerable<UserResponseModel>>.Success(users.ToList()));
    }


    #endregion
}
