﻿using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Solidaridad.Application.Common.Email;
using Solidaridad.Application.Common.PortalSettings;
using Solidaridad.Application.Exceptions;
using Solidaridad.Application.Helpers;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.Country;
using Solidaridad.Application.Models.User;
using Solidaridad.Application.Templates;
using Solidaridad.Core.Entities;
using Solidaridad.Core.Entities.Base;
using Solidaridad.DataAccess.Identity;
using Solidaridad.DataAccess.Persistence.Seeding.Permission;
using Solidaridad.DataAccess.Repositories;
using System.Collections;
using System.Web;

namespace Solidaridad.Application.Services.Impl;

public class AccountService : IAccountService
{
    #region DI
    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailService;
    private readonly IMapper _mapper;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ITemplateService _templateService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserRepository _userRepository;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IPermissionService _permissionService;
    private readonly IAccessLogRepository _accessLogRepository;
    private readonly PortalSettings _portalSettings;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public AccountService(IMapper mapper,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
         RoleManager<ApplicationRole> roleManager,
        IConfiguration configuration,
        ITemplateService templateService,
        IEmailService emailService,
        IUserRepository userRepository,
        IPermissionService permissionService,
        IAccessLogRepository accessLogRepository,
        PortalSettings portalSettings)
    {
        _mapper = mapper;
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _templateService = templateService;
        _emailService = emailService;
        _userRepository = userRepository;
        _roleManager = roleManager;
        _permissionService = permissionService;
        _accessLogRepository = accessLogRepository;
        _portalSettings = portalSettings;
    }
    #endregion

    #region Methods
    public async Task<IEnumerable<LoginUserModel>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync(c => 1 == 1);

        return _mapper.Map<IEnumerable<LoginUserModel>>(users);
    }

    public async Task<CreateUserResponseModel> CreateAsync(CreateUserModel createUserModel)
    {
        var user = _mapper.Map<ApplicationUser>(createUserModel);

        var result = await _userManager.CreateAsync(user, createUserModel.Password);

        if (!result.Succeeded) throw new BadRequestException(result.Errors.FirstOrDefault()?.Description);

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        var emailTemplate = await _templateService.GetTemplateAsync(TemplateConstants.ConfirmationEmail);

        var emailBody = _templateService.ReplaceInTemplate(emailTemplate,
            new Dictionary<string, string> { { "{UserId}", user.Id }, { "{Token}", token } });

        await _emailService.SendEmailAsync(EmailMessage.Create(user.Email, emailBody, "SolidaridadConfirm your email"));

        return new CreateUserResponseModel
        {
            Id = Guid.Parse((await _userManager.FindByNameAsync(user.UserName)).Id)
        };
    }

    public async Task<LoginResponseModel> LoginAsync(LoginUserModel loginUserModel, string ip, string userAgent)
    {
        var permissions = new List<string>();
        var roles = new List<string>();

        var user = _userManager.Users.FirstOrDefault(u =>
                    (u.UserName == loginUserModel.Username ||
                    u.Email == loginUserModel.Email) &&
                    u.IsActive == true);

        if (user == null)
            throw new NotFoundException("Username or password is incorrect");

        if (user.IsLoginEnabled == false)
            throw new BadRequestException("You do not have access to this portal.");

        var signInResult = await _signInManager.CheckPasswordSignInAsync(user, loginUserModel.Password, false);

        if (!signInResult.Succeeded)
            throw new BadRequestException("Username or password is incorrect");

        var validTo = DateTime.UtcNow.AddHours(2);
        var token = JwtHelper.GenerateToken(user, _configuration, validTo);
        var refreshToken = JwtHelper.GenerateRefreshToken(user, _configuration);

        // Get user roles and permissions from Identity claims
        var userRoles = await _userManager.GetRolesAsync(user);
        roles.AddRange(userRoles);
        
        foreach (var roleName in userRoles)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role != null)
            {
                var claims = await _roleManager.GetClaimsAsync(role);
                var permissionClaims = claims.Where(c => c.Type == "Permission").Select(c => c.Value);
                permissions.AddRange(permissionClaims);
            }
        }
        
        // Remove duplicate permissions
        permissions = permissions.Distinct().ToList();

        var userCountries = _mapper.Map<IEnumerable<CountryResponseModel>>(await _userRepository.GetUserCountries(user.Id));
        var countryId = (userCountries != null && userCountries.Count() > 0) ? userCountries.FirstOrDefault()?.Id : null;

        // access log
        await SaveAccessLog(ip, userAgent, countryId, user);

        // send OTP to user email (non-blocking - don't fail login if email fails)
        var otp = await CreateAndStoreOtpAsync(user.Id);
        var emailBody = $"Your OTP is {otp}. It will expire in 5 minutes.";
        
        // Log OTP to console for development/debugging
        Console.WriteLine($"[OTP] User: {user.Email}, OTP: {otp}");
        
        // Send email in background - don't block login if email fails
        _ = Task.Run(async () =>
        {
            try
            {
                await _emailService.SendEmailAsync(EmailMessage.Create(user.Email, emailBody, "Solidaridad - Your OTP Code"));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[OTP Email Error] Failed to send OTP email to {user.Email}: {ex.Message}");
            }
        });

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
            Countries = userCountries,
            RequiresPasswordChange = user.ForcePasswordChange,
            UserId = user.Id
        };
    }

    private async Task SaveAccessLog(string ip, string userAgent, Guid? countryId, ApplicationUser user)
    {
        await _accessLogRepository.AddAsync(new Core.Entities.AccessLog
        {
            UserId = user.Id.ToString(),
            UserName = user.UserName,
            AccessTime = DateTime.UtcNow,
            AccessType = AccessType.Login,
            Status = AccessStatus.Success,
            IpAddress = ip,
            UserAgent = userAgent,
            CountryId = countryId ?? Guid.Empty
        });
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

        return new BaseResponseModel
        {
            Id = Guid.Parse(user.Id)
        };
    }

    public async Task<UserResponseModel> GetFirstAsync(Guid id)
    {
        try
        {
            var user = await _userRepository.GetAllAsync(c => 1 == 1);

            return _mapper.Map<UserResponseModel>(user.FirstOrDefault());
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    public async Task<IEnumerable<string>> GetPermissionsAsync(string username, Guid? countryId)
    {
        try
        {
            var permissions = new List<string>();

            var user = _userManager.Users.FirstOrDefault(u =>
                        u.UserName == username);

            if (user == null)
            {
                Console.WriteLine($"User not found: {username}");
                return permissions;
            }

            // Get all roles for the user
            var userRoles = await _userManager.GetRolesAsync(user);
            
            Console.WriteLine($"User {username} has {userRoles.Count} roles: {string.Join(", ", userRoles)}");

            // Get permissions from Identity role claims (not RolePermission table)
            foreach (var roleName in userRoles)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role != null)
                {
                    // Check if role matches country (if countryId is provided and role has CountryId)
                    // If countryId is null or role.CountryId is null, include the role
                    bool includeRole = countryId == null || role.CountryId == null || countryId == role.CountryId;
                    
                    if (includeRole)
                    {
                        var claims = await _roleManager.GetClaimsAsync(role);
                        var permissionClaims = claims.Where(c => c.Type == "Permission").Select(c => c.Value);
                        permissions.AddRange(permissionClaims);
                        Console.WriteLine($"Role {roleName} has {permissionClaims.Count()} permissions");
                    }
                }
            }

            // Remove duplicates
            var distinctPermissions = permissions.Distinct().ToList();
            Console.WriteLine($"Total distinct permissions for {username}: {distinctPermissions.Count}");
            
            return distinctPermissions;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetPermissionsAsync for {username}: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            throw ex;
        }
    }

    public async Task<IEnumerable<AccessLog>> GetAcessLogAsync(SearchParams searchParams)
    {
        var _accessLogs = await _accessLogRepository.GetAllAsync(c =>
           //(string.IsNullOrEmpty(searchParams.Filter) ||
           // c.UserName.Contains(searchParams.Filter) ||
           // c.UserAgent.Contains(searchParams.Filter)
           c.CountryId == searchParams.CountryId
           && c.IsDeleted == false
       );

        if (_accessLogs != null && _accessLogs.Any())
        {
            int numberOfObjectsPerPage = searchParams.PageSize;

            var queryResultPage = _accessLogs
                .Skip(numberOfObjectsPerPage * (searchParams.PageNumber - 1))
                .Take(numberOfObjectsPerPage);

            var list = queryResultPage.ToList();

            return list.OrderByDescending(c => c.AccessTime);
        }

        return new List<AccessLog>();
    }

    public async Task<string> CreateAndStoreOtpAsync(string userId)
    {
        return await _userRepository.CreateAndStoreOtpAsync(userId);
    }

    public async Task<bool> VerifyOtp(string userId, string enteredOtp)
    {
        return await _userRepository.VerifyOtpAsync(userId, enteredOtp);
    }
    #endregion
}
