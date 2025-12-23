using DotNetEnv;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Solidaridad.DataAccess.Identity;
using Solidaridad.API;
using Solidaridad.API.Extensions;
using Solidaridad.API.Filters;
using Solidaridad.API.Middleware;
using Solidaridad.Application;
using Solidaridad.Application.Models.Validators;
using Solidaridad.DataAccess;
using Solidaridad.DataAccess.Persistence;
using Quartz;
using Solidaridad.Application.Services.Jobs;
using QuestPDF.Infrastructure;

// Load .env from solution root (paymentsystem-apis)
// 1. Calculate the robust path to the .env file in the root directory (paymentsystem-apis)
var currentDir = Directory.GetCurrentDirectory();
var envPath = Path.Combine(currentDir, "..", "..", ".env");

// 2. Load the .env file using the correct static class `Env`
if (File.Exists(envPath))
{
    // Load() will set environment variables that ASP.NET Core can pick up immediately.
    Env.Load(envPath);
    Console.WriteLine($"[Config] Successfully loaded environment variables from: {envPath}");
}
else
{
    Console.WriteLine($"[Config] .env file not found at: {envPath}. Relying on system/appsettings configuration.");
}

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers(
    config => config.Filters.Add(typeof(ValidateModelAttribute))
);

builder.Services.AddQuartz(q =>
{
    q.SchedulerId = "Scheduler-Core";
    q.UseMicrosoftDependencyInjectionJobFactory();

    
    var jobKey = new JobKey("LoanInterestJob");
    q.AddJob<LoanInterestJob>(opts => opts.WithIdentity(jobKey));

    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("LoanInterestJob-trigger").WithCronSchedule("0 0 8 1 * ?")); // Runs on the 1st of every month at 8 AM  .WithCronSchedule("0 0 */2 * * ?")
});




builder.Services.AddQuartz(q =>
{
    // MicrosoftDependencyInjectionJobFactory is now the default, no need to call UseMicrosoftDependencyInjectionJobFactory()

    var jobKey = new JobKey("MonthlyLoanStatementJob");

    q.AddJob<MonthlyLoanStatementJob>(opts => opts.WithIdentity(jobKey));

    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("MonthlyLoanStatementTrigger")
        .WithCronSchedule("0 0 0 1 * ?") // At 00:00 on day 1 of every month
    );
});

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

QuestPDF.Settings.License = LicenseType.Community;
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining(typeof(IValidationsMarker));

builder.Services.AddSwagger();



// Add data protection services first (required for DataProtectorTokenProvider)
builder.Services.AddDataProtection();

builder.Services.AddDataAccess(builder.Configuration)
    .AddApplication(builder.Environment);

builder.Services.AddJwt(builder.Configuration);

builder.Services.AddEmailConfiguration(builder.Configuration);

builder.Services.AddPortalConfiguration(builder.Configuration);


//builder.WebHost.UseKestrel(options =>
//{
//    options.AddServerHeader = false;
//});

var app = builder.Build();

using var scope = app.Services.CreateScope();

// Seed database
await DatabaseContextSeed.SeedDatabaseAsync(
    scope.ServiceProvider.GetRequiredService<DatabaseContext>(),
    scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>(),
    scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>()
);

// await AutomatedMigration.MigrateAsync(scope.ServiceProvider);

// app.UseSwaggerAuthorized();
app.UseSwagger();
app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Solidaridad API V1"); });

// CORS must come before HTTPS redirection
app.UseRouting();

app.UseCors(corsPolicyBuilder =>
    corsPolicyBuilder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
);

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseStaticFiles();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<PerformanceMiddleware>();

app.UseMiddleware<CountryMiddleware>();

app.UseMiddleware<TransactionMiddleware>();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseMiddleware<HeaderRemovalMiddleware>();

app.MapControllers();




app.Run();

namespace Solidaridad.API
{
    public partial class Program { }
}
