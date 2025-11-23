using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Solidaridad.Application.Common.Email;
using Solidaridad.Application.Common.PortalSettings;
using Solidaridad.Application.Exceptions;
using Solidaridad.Application.Helpers;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.User;
using Solidaridad.Application.Templates;
using Solidaridad.Core.Entities;
using Solidaridad.Core.Entities.Base;
using Solidaridad.Core.Enums;
using Solidaridad.DataAccess.Identity;
using Solidaridad.DataAccess.Repositories;
using System.Collections;
using System.Security.Cryptography;
using System.Web;

namespace Solidaridad.Application.Services.Impl;

public class UserService : IUserService
{
    #region DI
    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailService;
    private readonly ICountryService _countryService;
    private readonly IMapper _mapper;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ITemplateService _templateService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserRepository _userRepository;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IPermissionService _permissionService;
    private readonly IPasswordResetTokenRepository _passwordResetTokenRepository;
    private readonly PortalSettings _portalSettings;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public UserService(IMapper mapper,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<ApplicationRole> roleManager,
        IConfiguration configuration,
        ITemplateService templateService,
        IEmailService emailService,
        ICountryService countryService,
        IPermissionService permissionService,
        IPasswordResetTokenRepository passwordResetTokenRepository,
        IUserRepository userRepository,
        IWebHostEnvironment webHostEnvironment,
        PortalSettings portalSettings)
    {
        _mapper = mapper;
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _templateService = templateService;
        _emailService = emailService;
        _permissionService = permissionService;
        _countryService = countryService;
        _userRepository = userRepository;
        _passwordResetTokenRepository = passwordResetTokenRepository;
        _portalSettings = portalSettings;
        _webHostEnvironment = webHostEnvironment;
    }
    #endregion

    #region Methods
    public async Task<IEnumerable<UserResponseModel>> GetAllAsync(string roles = "")
    {
        var users = await _userManager.Users.ToListAsync();
        var userList = _mapper.Map<IEnumerable<UserResponseModel>>(users);
        var countries = await _countryService.GetAllAsync();
        //var userCountries = await _userRepository.GetUserCountriesAll();

        foreach (var user in userList)
        {
            var userRoles = await _userManager.GetRolesAsync(new ApplicationUser { Id = user.Id.ToString() });
            user.RoleName = string.Join(", ", userRoles);
            user.RoleId = userRoles.Count > 0 ? _roleManager.FindByNameAsync(userRoles[0]).Result.Id : "";
            user.UserCountriesStr = await _userRepository.GetUserCountriesStr(user.Id.ToString());
        }
        // remove superadmin users
        var newList = userList.ToList();
        newList.RemoveAll(u => u.RoleName.IndexOf("SuperAdmin") > 0);

        if (string.IsNullOrEmpty(roles))
        {
            return newList.OrderBy(c => c.Username);
        }

        var filteredList = newList.Where(x => roles.Split(',').Select(s => s.Trim().ToLower()).Contains(x.RoleName.ToLower())).ToList();

        return filteredList.OrderBy(c => c.Username);
    }

    public async Task<CreateUserResponseModel> CreateAsync(CreateUserModel createUserModel, bool forcePasswordChange = false)
    {
        createUserModel.Password = "Password123!";
        var user = _mapper.Map<ApplicationUser>(createUserModel);

        user.EmailConfirmed = true;
        user.ForcePasswordChange = forcePasswordChange;

        string password = PasswordGenerator.GeneratePassword(8);
        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded) throw new BadRequestException(result.Errors.FirstOrDefault()?.Description);
        foreach (var roleName in createUserModel.RoleNames)
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }

        // var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        // assign countries
        await _userRepository.AddUserCountryAsync(user.Id, createUserModel.CountryIds);

        // send email only if user login is enabled
        if (user.IsLoginEnabled)
        {
            var emailTemplate = await _templateService.GetTemplateAsync(TemplateConstants.ConfirmationEmail);

            var emailBody = _templateService.ReplaceInTemplate(emailTemplate,
                new Dictionary<string, string> {
                { "{{UserName}}", user.UserName },
                { "{{UserEmail}}", user.Email },
                { "{{Password}}", password }
                });

            await _emailService.SendEmailAsync(EmailMessage.Create(user.Email, emailBody, "Solidaridad - Confirm your email"));
        }

        return new CreateUserResponseModel
        {
            Id = Guid.Parse((await _userManager.FindByNameAsync(user.UserName)).Id)
        };
    }

    public async Task<LoginResponseModel> LoginAsync(LoginUserModel loginUserModel)
    {
        var permissions = new List<string>();
        var roles = new List<string>();

        var user = _userManager.Users.FirstOrDefault(u =>
                    (u.UserName == loginUserModel.Username ||
                    u.Email == loginUserModel.Email) &&
                    u.IsActive == true);

        if (user == null)
            throw new NotFoundException("Username or password is incorrect");

        var signInResult = await _signInManager.PasswordSignInAsync(user, loginUserModel.Password, false, false);

        if (!signInResult.Succeeded)
            throw new BadRequestException("Username or password is incorrect");

        var validTo = DateTime.UtcNow.AddDays(7);
        var token = JwtHelper.GenerateToken(user, _configuration, validTo);
        var refreshToken = JwtHelper.GenerateRefreshToken(user, _configuration);

        foreach (var role in _roleManager.Roles.ToList())
        {
            if (await _userManager.IsInRoleAsync(user, role.Name))
            {
                var all = await _permissionService.GetAllAsync(role.Id, new Guid());
                permissions.AddRange(all.Where(c => c.Selected == true).Select(c => c.PermissionName));

                // add user role
                roles.Add(role.Name);
            }
        }

        return new LoginResponseModel
        {
            Username = user.UserName,
            Email = user.Email,
            api_token = token,
            refresh_token = refreshToken,
            expire_duration = validTo - DateTime.UtcNow,
            token_type = "Bearer",
            Permissions = permissions?.Distinct(),
            Roles = roles,
            RequiresPasswordChange = user.ForcePasswordChange
        };
    }

    public async Task<ConfirmEmailResponseModel> ConfirmEmailAsync(ConfirmEmailModel confirmEmailModel)
    {
        var user = await _userManager.FindByIdAsync(confirmEmailModel.UserId);

        if (user == null)
            throw new UnprocessableRequestException("Your verification link is incorrect");

        var result = await _userManager.ConfirmEmailAsync(user, confirmEmailModel.Token);

        if (!result.Succeeded)
            throw new UnprocessableRequestException("Your verification link has expired");

        return new ConfirmEmailResponseModel
        {
            Confirmed = true
        };
    }

    public async Task<BaseResponseModel> ChangePasswordAsync(Guid userId, ChangePasswordModel changePasswordModel)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user == null)
            throw new NotFoundException("User does not exist anymore");

        var result =
            await _userManager.ChangePasswordAsync(user, changePasswordModel.OldPassword,
                changePasswordModel.NewPassword);

        if (!result.Succeeded)
            throw new BadRequestException(result.Errors.FirstOrDefault()?.Description);

        user.ForcePasswordChange = false;

        var upresult = await _userManager.UpdateAsync(user);

        return new BaseResponseModel
        {
            Id = Guid.Parse(user.Id)
        };
    }

    public async Task<UserResponseModel> GetFirstAsync(Guid id)
    {
        try
        {
            var user = await _userRepository.GetAllAsync(c => c.Id == id);

            return _mapper.Map<UserResponseModel>(user.FirstOrDefault());
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    public async Task<BaseResponseModel> DeleteAsync(Guid id)
    {
        var user = await _userManager.Users.Where(c => c.Id == id.ToString()).FirstOrDefaultAsync();

        if (user == null)
            throw new BadRequestException("The selected user does not exist");

        return new BaseResponseModel
        {
            Id = (await _userManager.DeleteAsync(user)).Succeeded ? id : Guid.Empty,
        };
    }

    public async Task<UpdateUserResponseModel> UpdateAsync(Guid id, UpdateUserModel updateUserModel)
    {
        try
        {
            var user = await _userManager.Users.Where(c => c.Id == id.ToString()).FirstOrDefaultAsync();

            if (user == null)
                throw new BadRequestException("The selected user does not exist");

            user.UserName = updateUserModel.Username;
            user.PhoneNumber = updateUserModel.PhoneNumber;
            user.Email = updateUserModel.Email;
            // user.CountryId = updateUserModel.CountryId;
            user.ProjectId = updateUserModel.ProjectId;
            user.ProjectManagerId = updateUserModel.ProjectManagerId;
            user.IsLoginEnabled = updateUserModel.IsLoginEnabled;
            user.IsActive = updateUserModel.IsActive;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded) throw new BadRequestException(result.Errors.FirstOrDefault()?.Description);

            // Get the user's current roles
            var currentRoles = await _userManager.GetRolesAsync(user);

            // Remove the user from their current roles
            foreach (var role in currentRoles)
            {
                await _userManager.RemoveFromRoleAsync(user, role);
            }

            foreach (var roleName in updateUserModel.RoleNames)
            {
                await _userManager.AddToRoleAsync(user, roleName);
            }

            // assign countries
            await _userRepository.UpdateUserCountryAsync(user.Id, updateUserModel.CountryIds);

            return new UpdateUserResponseModel
            {
                Id = result.Succeeded ? id : Guid.Empty,
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<IEnumerable<SelectItemModel>> GetUserCountries(string userId)
    {
        var _farmerCoops = await _userRepository.GetUserCountries(userId);

        var result = from c in _farmerCoops
                     select new SelectItemModel
                     {
                         Label = c.CountryName,
                         Value = Convert.ToString(c.Id)
                     };

        return result.OrderBy(c => c.Label);
    }

    public async Task<IEnumerable<SelectItemModel>> GetUserRoles(string userId)
    {

        var roles = new List<dynamic>();
        var user = _userManager.Users.FirstOrDefault(u => u.Id == userId);


        if (user == null)
            throw new NotFoundException("User cannot be found ");
        foreach (var role in _roleManager.Roles.ToList())
        {
            if (await _userManager.IsInRoleAsync(user, role.Name))
            {


                // add user role
                roles.Add(role);
            }
        }

        var result = from c in roles
                     select new SelectItemModel
                     {
                         Label = c.Name,
                         Value = Convert.ToString(c.Id)
                     };

        return result.OrderBy(c => c.Label);
    }

    public async Task<BaseResponseModel> ForgotPasswordAsync(ForgotPasswordModel forgotPasswordModel)
    {
        var user = await _userManager.FindByEmailAsync(forgotPasswordModel.Email);

        if (user == null)
            throw new NotFoundException("A user with this email does not exist");

        //var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        var passwordResetToken = new PasswordResetToken
        {
            UserId = user.Id,
            Token = token,
            ExpiryDate = DateTime.UtcNow.AddMinutes(30), // Set token to expire in 30 minutes
            CreatedOn = DateTime.UtcNow,
            IsUsed = false
        };

        // save token
        await _passwordResetTokenRepository.AddAsync(passwordResetToken);

        var emailBody = GenerateResetPasswordEmailBody(token, user.Id);

        await _emailService.SendEmailAsync(EmailMessage.Create(user.Email, emailBody, "Reset your password"));

        return new BaseResponseModel
        {
            Id = Guid.Parse(user.Id)
        };
    }

    public async Task<BaseResponseModel> ResetPasswordAsync(ResetPasswordModel resetPasswordModel)
    {
        var user = await _userManager.FindByIdAsync(resetPasswordModel.UserId);

        if (user == null)
            throw new NotFoundException("A user with this email does not exist");

        var validToken = await _passwordResetTokenRepository.GetAllAsync(t =>
             t.UserId == user.Id
                     && t.Token == resetPasswordModel.Token
                     && t.ExpiryDate > DateTime.UtcNow
                     && !t.IsUsed);

        if (validToken != null && validToken.Any())
        {
            var passwordHash = _userManager.PasswordHasher.HashPassword(user, resetPasswordModel.Password);
            user.PasswordHash = passwordHash;
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                var passwordResetToken = validToken.Where(ti => ti.Token == resetPasswordModel.Token).FirstOrDefault();
                if (passwordResetToken != null)
                {
                    passwordResetToken.IsUsed = true;
                    await _passwordResetTokenRepository.UpdateAsync(passwordResetToken);
                }
            }
            else
            {
                throw new BadRequestException("Unable to update password. Please re-try");
            }
        }
        else
        {
            throw new BadRequestException("Invalid token or the token might have expired.");
        }
        return new BaseResponseModel
        {
            Id = Guid.Parse(user.Id)
        };
    }

    #endregion

    private string GenerateResetPasswordEmailBody(string api_token, string userId)
    {
        try
        {
            string encodedToken = $"{_portalSettings.PortalUrl}/auth/reset-password/change?u={userId}&t={HttpUtility.UrlEncode(api_token)}";
            Hashtable ht = new Hashtable
            {
                { "@AppName", _portalSettings.AppName },
                { "@ResetLink", encodedToken}
            };

            string templateName = Templates.TemplateConstants.ResetPassword;

            string fileName = Path.Combine(_webHostEnvironment.ContentRootPath, $"wwwroot\\emailtemplates\\{templateName}");

            return Utility.GetContentFromTemplate(ht, fileName);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<IEnumerable<UserResponseModel>> GetAllAuthorisedAsync(AuthorisedUsersSearchParams authorisedUsersSearchParams)
    {
        var users = await _userManager.Users.ToListAsync();
        var requiredPermission = authorisedUsersSearchParams.Permission == "Review" ? "payments.batch.review" : "payments.batch.approve";
        var userList = _mapper.Map<IEnumerable<UserResponseModel>>(users);
        var authorisedUsers = new List<UserResponseModel>();
        var countries = await _countryService.GetAllAsync();

        var allRoles = await _roleManager.Roles.Where(c =>
                     c.CountryId == authorisedUsersSearchParams.CountryId &&
                    !c.Name.Equals(Roles.SuperAdmin.ToString())).ToListAsync();
        //var userCountries = await _userRepository.GetUserCountriesAll();

        foreach (var user in userList)
        {
            var applicationUser = new ApplicationUser { Id = user.Id.ToString() };
            var userRoles = await _userManager.GetRolesAsync(applicationUser);

            var filteredRoles = allRoles.Where(r => userRoles.Contains(r.Name)).ToList();


            user.RoleName = string.Join(", ", filteredRoles);
            user.RoleId = userRoles.Count > 0 ? _roleManager.FindByNameAsync(userRoles[0]).Result.Id : "";
            user.UserCountriesStr = await _userRepository.GetUserCountriesStr(user.Id.ToString());


            var permissions = new List<string>();
            foreach (var roleName in userRoles)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role != null)
                {
                    var allPermissions = await _permissionService.GetAllAsync(role.Id, Guid.Empty);
                    var selectedPermissions = allPermissions
                        .Where(p => p.Selected)
                        .Select(p => p.PermissionName);

                    permissions.AddRange(selectedPermissions);
                }
            }
            if (permissions.Contains(requiredPermission))
            {
                authorisedUsers.Add(user);
            }
            //user.Permissions = permissions.Distinct().ToList(); // Ensure you add this property in UserResponseModel
        }

        // Remove superadmin users

        authorisedUsers.RemoveAll(u => u.RoleName.IndexOf("SuperAdmin", StringComparison.OrdinalIgnoreCase) > 0);

        if (string.IsNullOrEmpty(authorisedUsersSearchParams.Permission))
        {
            return authorisedUsers.OrderBy(c => c.Username);
        }

        var filteredList = authorisedUsers
            .Where(x => authorisedUsersSearchParams.Permission.Split(',').Select(s => s.Trim().ToLower()).Contains(x.RoleName.ToLower()))
            .OrderBy(c => c.Username)
            .ToList();

        return filteredList;
    }

    public async Task<IEnumerable<UserResponseModel>> GetOfficerList(Guid? countryId)
    {
        var users = await _userManager.Users.ToListAsync();
        var requiredPermission = "loans.applications.view";
        var userList = _mapper.Map<IEnumerable<UserResponseModel>>(users);
        var authorisedUsers = new List<UserResponseModel>();
        var countries = await _countryService.GetAllAsync();

        var allRoles = await _roleManager.Roles
            .Where(c => c.CountryId == countryId && !c.Name.Equals(Roles.SuperAdmin.ToString()))
            .ToListAsync();

        // Cache role details and permissions
        var rolePermissionCache = new Dictionary<string, (string RoleId, List<string> Permissions)>();
        foreach (var role in allRoles)
        {
            var allPermissions = await _permissionService.GetAllAsync(role.Id, Guid.Empty);
            var selectedPermissions = allPermissions
                .Where(p => p.Selected)
                .Select(p => p.PermissionName)
                .ToList();
            rolePermissionCache[role.Name] = (role.Id, selectedPermissions);
        }

        foreach (var user in userList)
        {
            var applicationUser = new ApplicationUser { Id = user.Id.ToString() };
            var userRoles = await _userManager.GetRolesAsync(applicationUser);

            var filteredRoles = allRoles.Where(r => userRoles.Contains(r.Name)).ToList();
            user.RoleName = string.Join(", ", filteredRoles.Select(r => r.Name));  // fixed RoleName logic
            user.RoleId = userRoles.FirstOrDefault() != null && rolePermissionCache.ContainsKey(userRoles[0])
                ? rolePermissionCache[userRoles[0]].RoleId
                : "";

            user.UserCountriesStr = await _userRepository.GetUserCountriesStr(user.Id.ToString());

            var permissions = userRoles
                .Where(rolePermissionCache.ContainsKey)
                .SelectMany(r => rolePermissionCache[r].Permissions)
                .Distinct();

            if (permissions.Contains(requiredPermission))
            {
                authorisedUsers.Add(user);
            }
        }

        authorisedUsers.RemoveAll(u =>
            u.RoleName.IndexOf("SuperAdmin", StringComparison.OrdinalIgnoreCase) > 0);

        return authorisedUsers.OrderBy(c => c.Username).ToList();
    }
}

