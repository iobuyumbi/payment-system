using Solidaridad.Application.Models;
using Solidaridad.Application.Models.User;
using Solidaridad.Core.Entities;
using Solidaridad.Core.Entities.Base;

namespace Solidaridad.Application.Services;

public interface IAccountService
{
    Task<BaseResponseModel> ChangePasswordAsync(Guid userId, ChangePasswordModel changePasswordModel);

    Task<ConfirmEmailResponseModel> ConfirmEmailAsync(ConfirmEmailModel confirmEmailModel);

    Task<CreateUserResponseModel> CreateAsync(CreateUserModel createUserModel);

    Task<IEnumerable<LoginUserModel>> GetAllAsync();

    Task<LoginResponseModel> LoginAsync(LoginUserModel loginUserModel, string ip, string userAgent);

    Task<IEnumerable<string>> GetPermissionsAsync(string username, Guid? countryId);

    Task<UserResponseModel> GetFirstAsync(Guid userId);

    Task<IEnumerable<AccessLog>> GetAcessLogAsync(SearchParams searchParams);

    Task<bool> VerifyOtp(string userId, string enteredOtp);

    Task<string> CreateAndStoreOtpAsync(string userId);
}
