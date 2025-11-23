using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Solidaridad.Application.Common.Email;
using Solidaridad.Application.Common.PortalSettings;
using Solidaridad.Application.MappingProfiles;
using Solidaridad.Application.Services;
using Solidaridad.Application.Services.Impl;
using Solidaridad.Shared.Services;
using Solidaridad.Shared.Services.Impl;

namespace Solidaridad.Application;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IWebHostEnvironment env)
    {
        services.AddServices(env);

        services.RegisterAutoMapper();

        return services;
    }

    private static void AddServices(this IServiceCollection services, IWebHostEnvironment env)
    {
        services.AddMemoryCache();

        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.TryAddScoped<SignInManager<DataAccess.Identity.ApplicationUser>>();

        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IClaimService, ClaimService>();
        services.AddScoped<ITemplateService, TemplateService>();
        services.AddScoped<ICountryService, CountryService>();
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<IFarmerService, FarmerService>();
        services.AddScoped<ILoanApplicationService, LoanApplicationService>();
        services.AddScoped<IExcelImportService, ExcelImportService>();
        services.AddScoped<IMasterLoanItemService, MasterLoanItemService>();
        services.AddScoped<ICooperativeService, CooperativeService>();
        services.AddScoped<IAdminLevel1Service, AdminLevel1Service>();
        services.AddScoped<IAdminLevel2Service, AdminLevel2Service>();
        services.AddScoped<IAdminLevel4Service, AdminLevel4Service>();
        services.AddScoped<IAdminLevel3Service, AdminLevel3Service>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ILoanBatchService, LoanBatchService>();
        services.AddScoped<ILoanItemService, LoanItemService>();
        services.AddScoped<IItemCategoryService, ItemCategoryService>();
        services.AddScoped<ILoanCategoryService, LoanCategoryService>();
        services.AddScoped<IAttachmentMappingService, AttachmentMappingService>();
        services.AddScoped<IAttachmentUploadService, AttachmentUploadService>();
        services.AddScoped<IApplicationStatusService, ApplicationStatusService>();
        services.AddScoped<IPaymentBatchService, PaymentBatchService>();
        services.AddScoped<IAssociateService, AssociateService>();
        services.AddScoped<IDisbursementService, DisbursementService>();
        services.AddScoped<IWalletService, WalletService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IFacilitationService, FacilitationService>();
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<ILoanProcessingFeeService, LoanProcessingFeeService>();
        services.AddScoped<IEmailTemplateService, EmailTemplateService>();
        services.AddScoped<IMasterLoanTermService, MasterLoanTermService>();
        services.AddScoped<IExcelExportService, ExcelExportService>();
        services.AddScoped<IPaymentDeductibleService, PaymentDeductibleService>();
        services.AddScoped<IActivityLogService, ActivityLogService>();
        services.AddScoped<ILoanRepaymentService, LoanRepaymentService>();
        services.AddScoped<IDocumentTypeService, DocumentTypeService>();
        services.AddScoped<IAuditLogService, AuditLogService>();
        services.AddScoped<ICallBackService, CallBackService>();
        services.AddScoped<ILocationService, LocationService>();
        services.AddScoped<ILocationProfileService, LocationProfileService>();
        
        // services.AddScoped<IApiRequestLogger, ApiRequestLogger>();

        //if (env.IsDevelopment())
        //    services.AddScoped<IEmailService, DevEmailService>();
        //else
        services.AddScoped<IEmailService, EmailService>();
        services.AddHttpClient<ApiService>();
    }

    private static void RegisterAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(IMappingProfilesMarker));
    }

    public static void AddEmailConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(configuration.GetSection("SmtpSettings").Get<SmtpSettings>());
    }
    public static void AddPortalConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(configuration.GetSection("PortalSettings").Get<PortalSettings>());
    }
}
