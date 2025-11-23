using Quartz;
using Solidaridad.Core.Entities;
using Solidaridad.DataAccess.Repositories;

namespace Solidaridad.Application.Services.Jobs;


public class LoanInterestJob : IJob
{
    IJobExecutionLogRepository _jobExecutionLogRepository;

    public LoanInterestJob(IJobExecutionLogRepository jobExecutionLogRepository)
    {
        _jobExecutionLogRepository = jobExecutionLogRepository;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var jobName = context.JobDetail.Key.Name;
        var startTime = DateTime.UtcNow;

        // Create a new log entry
        var logEntry = new JobExecutionLog
        {
            JobName = jobName,
            StartTime = startTime,
            IsSuccess = false // Default to false until we know the outcome
        };

        try
        {
            // Logic to calculate loan interest and update principal
            Console.WriteLine("Calculating loan interest and updating principal...");

            // Example: Fetch loans from database and update them
            // var loans = await loanRepository.GetAllLoansAsync();
            // foreach (var loan in loans)
            // {
            //     loan.Principal += loan.Principal * loan.InterestRate; // Example calculation
            //     await loanRepository.UpdateLoanAsync(loan);
            // }

            // Mark as success
            logEntry.IsSuccess = true;
        }
        catch (Exception ex)
        {
            // Log the error message
            logEntry.ErrorMessage = ex.Message;
            Console.WriteLine($"Error: {ex.Message}");
        }
        finally
        {
            logEntry.EndTime = DateTime.UtcNow;
            await _jobExecutionLogRepository.AddAsync(logEntry);
        }

        Console.WriteLine("Job execution logged.");
    }
}
