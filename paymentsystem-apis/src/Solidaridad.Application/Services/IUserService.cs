using Solidaridad.Application.Models;
using Solidaridad.Application.Models.User;
using Solidaridad.Core.Entities.Base;

namespace Solidaridad.Application.Services;

public interface IUserService
{
    Task<BaseResponseModel> ChangePasswordAsync(Guid userId, ChangePasswordModel changePasswordModel);

    Task<ConfirmEmailResponseModel> ConfirmEmailAsync(ConfirmEmailModel confirmEmailModel);

    Task<CreateUserResponseModel> CreateAsync(CreateUserModel createUserModel, bool forcePasswordChange = false);

    Task<IEnumerable<UserResponseModel>> GetAllAsync(string roles = "");

    Task<IEnumerable<UserResponseModel>> GetAllAuthorisedAsync(AuthorisedUsersSearchParams authorisedUsersSearchParams);
    
    Task<IEnumerable<UserResponseModel>> GetOfficerList(Guid? CountryId);

    Task<LoginResponseModel> LoginAsync(LoginUserModel loginUserModel);

    Task<UserResponseModel> GetFirstAsync(Guid userId);

    Task<UpdateUserResponseModel> UpdateAsync(Guid id, UpdateUserModel updateUserModel);

    Task<BaseResponseModel> DeleteAsync(Guid id);
    
    Task<IEnumerable<SelectItemModel>> GetUserCountries(string userId);
    
    Task<IEnumerable<SelectItemModel>> GetUserRoles(string userId);
    
    Task<BaseResponseModel> ForgotPasswordAsync(ForgotPasswordModel forgotPasswordModel);
    
    Task<BaseResponseModel> ResetPasswordAsync(ResetPasswordModel resetPasswordModel);
  
    // Task<BaseResponseModel> ResetPasswordByAdmin(string userId, ChangePasswordModel changePasswordModel);
}
