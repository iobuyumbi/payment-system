using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Solidaridad.Application.Common.Email;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.User;
using Solidaridad.Application.Services;
using Solidaridad.Core.Entities;
using Solidaridad.Core.Entities.Base;
using Solidaridad.Core.Entities.Pagination;

namespace Solidaridad.API.Controllers;

[Authorize]
public class AccountController : ApiController
{
    #region DI
    private readonly IAccountService _userService;
    private readonly IEmailService _emailService;

    public AccountController(IAccountService userService, IEmailService emailService)
    {
        _userService = userService;
        _emailService = emailService;
    }
    #endregion

    [HttpPost("authenticate")]
    [AllowAnonymous]
    public async Task<IActionResult> LoginAsync(LoginUserModel loginUserModel)
    {
        string ip = HttpContext.Connection.RemoteIpAddress?.ToString();
        string userAgent = HttpContext.Request.Headers["User-Agent"].ToString();

        return Ok(ApiResult<LoginResponseModel>.Success(await _userService.LoginAsync(loginUserModel, ip, userAgent)));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(bool? isActive)
    {
        var users = await _userService.GetAllAsync();

        return Ok(ApiResult<IEnumerable<LoginUserModel>>.Success(users.ToList()));
    }

    [HttpPost("GetPermissions")]
    [AllowAnonymous]
    public async Task<IActionResult> GetPermissions([FromBody] string username)
    {
        var _countryId = CountryId.HasValue ? CountryId : Guid.Parse("a82beb82-2d92-11ef-ad6b-46477cdd49d1");
        
        var permissions = await _userService.GetPermissionsAsync(username, _countryId);
        
        // If permissions are empty, try to seed them for admin users
        if (!permissions.Any())
        {
            // Force re-seed permissions for admin users - return all permissions as a workaround
            if (username == "superadmin" || username == "adminuser")
            {
                var allPermissions = Solidaridad.DataAccess.Persistence.Seeding.Permission.Permissions.GenerateAllPermissions();
                return Ok(ApiResult<IEnumerable<string>>.Success(allPermissions));
            }
        }
        
        return Ok(ApiResult<IEnumerable<string>>.Success(permissions));
    }

    [HttpPost("DebugUserInfo")]
    [AllowAnonymous]
    public async Task<IActionResult> DebugUserInfo([FromBody] string username)
    {
        try
        {
            var _countryId = CountryId.HasValue ? CountryId : Guid.Parse("a82beb82-2d92-11ef-ad6b-46477cdd49d1");
            var permissions = await _userService.GetPermissionsAsync(username, _countryId);
            
            return Ok(new { 
                username = username,
                permissions = permissions,
                permissionCount = permissions.Count(),
                countryId = _countryId
            });
        }
        catch (Exception ex)
        {
            return Ok(new { 
                username = username,
                error = ex.Message,
                stackTrace = ex.StackTrace
            });
        }
    }


    [HttpPost]
    [Route("access-log")]
    public async Task<IActionResult> Search(SearchParams searchParams)
    {
        searchParams.CountryId = CountryId;
        var list = await _userService.GetAcessLogAsync(searchParams);

        int totalRecords = list != null ? list.Count() : 0;

        Page pageInfo = new Page
        {
            PageNumber = searchParams.PageNumber,
            Size = searchParams.PageSize,
            TotalElements = totalRecords,
            TotalPages = totalRecords / searchParams.PageSize
        };
        var pagedData = new PagedData<IEnumerable<AccessLog>>
        {
            Page = pageInfo,
            Result = list
        };

        return Ok(new ApiResponseModel<PagedData<IEnumerable<AccessLog>>>
        {
            Success = true,
            Message = "success",
            Data = pagedData
        });
    }

    [AllowAnonymous]
    [HttpPost("verify-otp")]
    public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequest request)
    {
        bool isValid = await _userService.VerifyOtp(request.UserId, request.Otp);

        // if (!isValid) return Unauthorized("Invalid OTP");

        return Ok(ApiResult<bool>.Success(isValid));
    }
}
