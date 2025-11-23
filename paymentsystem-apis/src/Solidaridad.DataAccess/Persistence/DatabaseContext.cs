using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.DependencyInjection;
using Solidaridad.Core.Common;
using Solidaridad.Core.Entities;
using Solidaridad.Core.Entities.Loans;
using Solidaridad.Core.Entities.Payments;
using Solidaridad.DataAccess.Identity;
using Solidaridad.Shared.Services;
using System.Reflection;

namespace Solidaridad.DataAccess.Persistence;

public class DatabaseContext : IdentityDbContext<ApplicationUser>
{
    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<ApplicationRole> AspNetRoles { get; set; }

    public DbSet<Address> Addresses { get; set; }

    public DbSet<Farmer> Farmers { get; set; }

    public DbSet<Country> Countries { get; set; }

    public DbSet<AdminLevel1> AdminLevel1 { get; set; }

    public DbSet<AdminLevel2> AdminLevel2 { get; set; }

    public DbSet<AdminLevel3> AdminLevel3 { get; set; }

    public DbSet<AdminLevel4> AdminLevel4 { get; set; }

    public DbSet<Project> Projects { get; set; }

    public DbSet<ExcelImport> ExcelImport { get; set; }

    public DbSet<LoanApplication> LoanApplications { get; set; }

    public DbSet<Constituency> Constituencies { get; set; }

    public DbSet<MasterLoanItem> LoanItems { get; set; }

    public DbSet<ItemCategory> ItemCategories { get; set; }

    public DbSet<LoanCategory> LoanCategories { get; set; }

    public DbSet<Cooperative> Cooperatives { get; set; }

    public DbSet<FarmerCooperative> FarmerCooperative { get; set; }

    public DbSet<AttachmentMapping> AttachmentMappings { get; set; }

    public DbSet<AttachmentFile> AttachmentFile { get; set; }

    public DbSet<ApplicationStatus> ApplicationStatus { get; set; }

    public DbSet<PaymenBatchLoanBatchMapping> PaymenBatchLoanBatchMapping { get; set; }

    public DbSet<MasterLoanTermAdditionalFee> MasterLoanTermAdditionalFee { get; set; }

    public DbSet<PaymentImportSummary> PaymentImportSummary { get; set; }

    public DbSet<MasterPaymentApprovalStage> MasterPaymentApprovalStage { get; set; }

    public DbSet<MasterLoanAppStage> MasterLoanAppStage { get; set; }

    public DbSet<PaymentBatchHistory> PaymentBatchHistory { get; set; }

    public DbSet<JobExecutionLog> JobExecutionLog { get; set; }

    public DbSet<DocumentType> DocumetType { get; set; }

    public DbSet<AuditLog> AuditLog { get; set; }

    public DbSet<UserCountry> UserCountry { get; set; }

    public DbSet<FarmerProject> FarmerProject { get; set; }

    public DbSet<LoanApplicationHistory> LoanApplicationHistory { get; set; }

    public DbSet<LoanApplicationImportStaging> LoanApplicationImportStaging { get; set; }

    public DbSet<LoanItemImportStaging> LoanItemImportStaging { get; set; }

    public DbSet<PasswordResetToken> PasswordResetToken { get; set; }

    public DbSet<UserOtp> UserOtp { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(DateTime))
                {
                    property.SetValueConverter(new ValueConverter<DateTime, DateTime>(
                        v => v.Kind == DateTimeKind.Utc ? v : v.ToUniversalTime(),
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                    ));
                }

                if (property.ClrType == typeof(DateTime?))
                {
                    property.SetValueConverter(new ValueConverter<DateTime?, DateTime?>(
                        v => v.HasValue ? (v.Value.Kind == DateTimeKind.Utc ? v.Value : v.Value.ToUniversalTime()) : v,
                        v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v
                    ));
                }
            }
        }

        builder.Entity<ApplicationUser>()
           .Property(u => u.IsActive)
           .HasDefaultValue(true);

        builder.Entity<ApplicationUser>()
          .Property(u => u.IsLoginEnabled)
          .HasDefaultValue(true);

        builder.Entity<Farmer>()
            .HasIndex(c => new { c.SystemId })
            .IsUnique()
            .HasFilter("\"IsDeleted\" = false AND \"SystemId\" IS NOT NULL AND \"SystemId\" <> ''");

        builder.Entity<Farmer>()
            .HasIndex(c => new { c.BeneficiaryId })
            .IsUnique()
            .HasFilter("\"IsDeleted\" = false AND \"BeneficiaryId\" IS NOT NULL AND \"BeneficiaryId\" <> ''");

        builder.Entity<Farmer>()
            .HasIndex(c => new { c.ParticipantId })
            .IsUnique()
            .HasFilter("\"IsDeleted\" = false AND \"ParticipantId\" IS NOT NULL AND \"ParticipantId\" <> ''");

        //builder.Entity<Farmer>()
        //    .HasIndex(c => new { c.Mobile })
        //    .IsUnique()
        //    .HasFilter("\"IsDeleted\" = false AND \"Mobile\" IS NOT NULL AND \"Mobile\" <> ''");

        //builder.Entity<Farmer>()
        //   .HasIndex(c => new { c.PaymentPhoneNumber })
        //   .IsUnique()
        //   .HasFilter("\"IsDeleted\" = false AND \"PaymentPhoneNumber\" IS NOT NULL AND \"PaymentPhoneNumber\" <> ''");

        builder.Entity<ApplicationStatusLog>()
           .HasOne(log => log.ApplicationStatus)
           .WithMany() // Or .WithMany(x => x.ApplicationStatusLogs) if reverse navigation is available
           .HasForeignKey(log => log.StatusId);

        base.OnModelCreating(builder);
    }

    public async Task<int> SaveChangesAsync(IServiceScopeFactory serviceScopeFactory, CancellationToken cancellationToken = new())
    {
        using IServiceScope scope = serviceScopeFactory.CreateScope();

        var _claimService = scope
            .ServiceProvider
            .GetRequiredService<IClaimService>();


        foreach (var entry in ChangeTracker.Entries<IAuditedEntity>())
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = new Guid(_claimService.GetUserId());
                    entry.Entity.CreatedOn = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedBy = new Guid(_claimService.GetUserId());
                    entry.Entity.UpdatedOn = DateTime.UtcNow;
                    break;
            }

        var auditLogs = new List<AuditLog>();

        //foreach (var entry in ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted))
        //{
        //    var tableName = entry.Entity.GetType().Name;
        //    var username = _claimService.GetUserId();

        //    if (entry.State == EntityState.Modified)
        //    {
        //        foreach (var property in entry.Properties)
        //        {
        //            if (property.IsModified)
        //            {
        //                var oldValue = property.OriginalValue?.ToString();
        //                var newValue = property.CurrentValue?.ToString();

        //                if (oldValue != newValue)
        //                {
        //                    auditLogs.Add(new AuditLog
        //                    {
        //                        TableName = tableName,
        //                        ColumnName = property.Metadata.Name,
        //                        OldValue = property.OriginalValue?.ToString(),
        //                        NewValue = property.CurrentValue?.ToString(),
        //                        DateTime = DateTime.UtcNow,
        //                        Username = username
        //                    });
        //                }
        //            }
        //        }
        //    }
        //    else if (entry.State == EntityState.Deleted)
        //    {
        //        auditLogs.Add(new AuditLog
        //        {
        //            TableName = tableName,
        //            ColumnName = "",
        //            OldValue = "",
        //            NewValue = "",
        //            DateTime = DateTime.UtcNow,
        //            Username = username
        //        });
        //    }
        //}

        await base.SaveChangesAsync(cancellationToken);

        //foreach (var auditLog in auditLogs)
        //{
        //    AuditLog.Add(auditLog);
        //}

        return await base.SaveChangesAsync(cancellationToken);
    }
}
