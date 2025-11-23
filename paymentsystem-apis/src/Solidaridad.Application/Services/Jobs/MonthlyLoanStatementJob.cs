using Microsoft.Extensions.Logging;
using Quartz;
using Solidaridad.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solidaridad.Application.Services.Jobs;

public class MonthlyLoanStatementJob : IJob
{
    private readonly ILoanRepaymentService _loanRepaymentService;
    private readonly ILoanBatchRepository _loanBatchRepository;
    private readonly ILoanApplicationService _loanApplicationService;
    private readonly ILogger<MonthlyLoanStatementJob> _logger;

    public MonthlyLoanStatementJob(ILoanRepaymentService loanRepaymentService,ILoanApplicationService loanApplicationService,ILoanBatchRepository loanBatchRepository, ILogger<MonthlyLoanStatementJob> logger)
    {
        _loanRepaymentService = loanRepaymentService;
        _logger = logger;

        _loanBatchRepository = loanBatchRepository;
        _loanApplicationService = loanApplicationService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            _logger.LogInformation("Starting Monthly Loan Statement Job at {time}", DateTime.Now);
            var loanBatches = await _loanBatchRepository.GetAllAsync(c=> c.IsDeleted == false);

            foreach (var batch in loanBatches)
            {
                var allLoanApplications = await _loanApplicationService.GetEffectiveLoanApplications(batch.Id);

                foreach (var app in allLoanApplications)
                {
                    try
                    {
                        await _loanRepaymentService.GenerateMonthlyLoanStatement(app.Id);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Failed for Application ID: {app.Id}");
                    }
                }
            }

            _logger.LogInformation("Completed Monthly Loan Statement Job at {time}", DateTime.Now);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to run Monthly Loan Statement Job");
        }
    }
}
