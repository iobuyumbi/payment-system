using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Solidaridad.Core.Entities;
using Solidaridad.DataAccess.Identity;
using Solidaridad.DataAccess.Persistence;
using Solidaridad.DataAccess.Repositories;
using Solidaridad.DataAccess.Repositories.Impl;

namespace Solidaridad.DataAccess;

public static class DataAccessDependencyInjection
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabase(configuration);

        services.AddIdentity();

        services.AddRepositories();

        return services;
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICountryRepository, CountryRepository>();
        services.AddScoped<IFarmerRepository, FarmerRepository>();
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IExcelImportRepository, ExcelImportRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ILoanApplicationRepository, LoanApplicationRepository>();
        services.AddScoped<IAdminLevel1Repository, AdminLevel1Repository>();
        services.AddScoped<IAdminLevel2Repository, AdminLevel2Repository>();
        services.AddScoped<IAdminLevel3Repository, AdminLevel3Repository>();
        services.AddScoped<IAdminLevel4Repository, AdminLevel4Repository>();
        services.AddScoped<IConstituencyRepository, ConstituencyRepository>();
        services.AddScoped<IAddressRepository, AddressRepository>();
        services.AddScoped<IMasterLoanItemRepository, MasterLoanItemRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddScoped<ICooperativeRepository, CooperativeRepository>();
        services.AddScoped<ILoanBatchRepository, LoanBatchRepository>();
        services.AddScoped<IFarmerCooperativeRepository, FarmerCooperativeRepository>();
        services.AddScoped<IItemCategoryRepository, ItemCategoryRepository>();
        services.AddScoped<ILoanCategoryRepository, LoanCategoryRepository>();
        services.AddScoped<ILoanBatchItemRepository, LoanBatchItemRepository>();
        services.AddScoped<ILoanItemRepository, LoanItemRepository>();
        services.AddScoped<IAttachmentMappingRepository, AttachmentMappingRepository>();
        services.AddScoped<IAttachmentUploadRepository, AttachmentUploadRepository>();
        services.AddScoped<IApplicationStatusRepository, ApplicationStatusRepository>();
        services.AddScoped<IPaymentBatchRepository, PaymentBatchRepository>();
        services.AddScoped<IAssociateRepository, AssociateRepository>();
        services.AddScoped<IDisbursementRepository, DisbursementRepository>();
        services.AddScoped<IApplicationStatusLogRepository, ApplicationStatusLogRepository>();
        services.AddScoped<IWalletRepository, WalletRepository>();
        services.AddScoped<IPaymentRequestDeductibleRepository, PaymentRequestDeductibleRepository>();
        services.AddScoped<IExcelImportDetailRepository, ExcelImportDetailRepository>();
        services.AddScoped<IPaymenBatchLoanBatchMappingRepository, PaymenBatchLoanBatchMappingRepository>();
        services.AddScoped<IPaymentBatchProjectMappingRepository, PaymentBatchProjectMappingRepository>();
        services.AddScoped<IFacilitationRepository, FacilitationRepository>();
        services.AddScoped<ILoanProcessingFeeRepository, LoanProcessingFeeRepository>();
        services.AddScoped<IEmailTemplateRepository, EmailTemplateRepository>();
        services.AddScoped<IEmailTemplateVariableRepository, EmailTemplateVariableRepository>();
        services.AddScoped<ILoanBatchProcessingFeeRepository, LoanBatchProcessingFeeRepository>();
        services.AddScoped<IMasterLoanTermRepository, MasterLoanTermRepository>();
        services.AddScoped<IMasterLoanTermsMappingRepository, MasterLoanTermsMappingRepository>();
        services.AddScoped<IActivityLogRepository, ActivityLogRepository>();
        services.AddScoped<IApiRequestLogRepository, ApiRequestLogRepository>();
        services.AddScoped<ILoanRepaymentRepository, LoanRepaymentRepository>();
        services.AddScoped<IJobExecutionLogRepository, JobExecutionLogRepository>();
        services.AddScoped<IEMIScheduleRepository, EMIScheduleRepository>();
        services.AddScoped<ILoanInterestRepository, LoanInterestRepository>();
        services.AddScoped<IDocumentTypeRepository, DocumentTypeRepository>();
        services.AddScoped<IAuditLogRepository, AuditLogRepository>();
        services.AddScoped<ICallBackRepository, CallBackRepository>();
        services.AddScoped<IModuleRepository, ModuleRepository>();
        services.AddScoped<IItemUnitRepository, ItemUnitRepository>();
        services.AddScoped<ILoanApplicationImportStagingRepository, LoanApplicationImportStagingRepository>();
        services.AddScoped<ILoanItemImportStagingRepository, LoanItemImportStagingRepository>();
        services.AddScoped<ILoanRepaymentScheduleRepository, LoanRepaymentScheduleRepository>();
        services.AddScoped<IBatchAttachmentMappingRepository, BatchAttachmentMappingRepository>();
        services.AddScoped<ILoanStatementRepository, LoanStatementRepository>();
        services.AddScoped<ILocationRepository, LocationRepository>();
        services.AddScoped<ILocationProfileRepository, LocationProfileRepository>();
        services.AddScoped<IPasswordResetTokenRepository, PasswordResetTokenRepository>();
        services.AddScoped<IPaymentDeductibleStatusMasterRepository, PaymentDeductibleStatusMasterRepository>();
        services.AddScoped<IAccessLogRepository, AccessLogRepository>();
    }

    private static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DatabaseContext>(options =>
        options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
    }

    private static void AddIdentity(this IServiceCollection services)
    {

        services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<DatabaseContext>();

        services.AddTransient<IUserTwoFactorTokenProvider<ApplicationUser>, PhoneNumberTokenProvider<ApplicationUser>>();
        services.AddTransient<IUserTwoFactorTokenProvider<ApplicationUser>, EmailTokenProvider<ApplicationUser>>();

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 1;

            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

            options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            options.User.RequireUniqueEmail = true;
        });
    }
}

// TODO move outside?
public class DatabaseConfiguration
{
    public string DefaultConnection { get; set; }
}
