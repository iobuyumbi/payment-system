using Solidaridad.Application.Models.LoanApplication;
using Solidaridad.Application.Models.Reports;
using Solidaridad.Core.Entities;
using Solidaridad.Core.Entities.Base;

namespace Solidaridad.Application.Services;

public interface IReportService
{
    Task<List<KeyMetricsModel>> GetStats(int year , Guid countryId);

    Task<IEnumerable<JobExecutionLog>> GetJobExecutionLog();
    #region PaymentBatch
    Task<PaymentConfirmationResponseModel> GetPaymentConfirmationReport(SearchParams searchParams);
    #endregion


    #region Loan Account Reports
    Task<LoanAccountResponseModel> GetLoanAccountReports(SearchParams searchParams);

    #endregion

    #region Loan Batch Reports / Loan Portfolio Reports
    Task<LoanBatchReportResponseModel> GetLoanBatchReports(SearchParams searchParams);
    Task<IEnumerable<RepaymentReportsResponseModel>> GetRepaymentReports(Guid? countryId);

    Task<IEnumerable<LoanApplicationResponseModel>> LoanApplicationReports(PortolioSearchParams portolioSearchParams);
    
    Task<byte[]> GenerateLoanPortfolioPdfReport(PortolioSearchParams searchParams);
    Task<IEnumerable<DisbursedLoanReportResponseModel>> DisbursedLoanReports(DisbursedSearchParams disbursedSearchParams);

    Task<byte[]> GenerateLoanDisbursementPdfReport(DisbursedSearchParams searchParams);

    Task<IEnumerable<PaymentReportResponseModel>> PaymentReports(PaymentReportSearchParams disbursedSearchParams);

    Task<byte[]> GeneratePaymentsPdfReport(PaymentReportSearchParams searchParams);

    Task<IEnumerable<LoanApplicationResponseModel>> CountryLoanApplicationReports(PortolioSearchParams portolioSearchParams);
    Task<byte[]> GenerateCountryLoanPortfolioPdfReport(PortolioSearchParams searchParams);

    Task<IEnumerable<GlobalLoanPortfolioReportResponseModel>> GlobalLoanPortfolioReports(PortolioSearchParams portolioSearchParams);
    Task<byte[]> GenerateGlobalLoanPortfolioPdfReport(PortolioSearchParams searchParams);



    Task<IEnumerable<GlobalLoanApplicationReportResponseModel>> GlobalLoanApplicationReports(PortolioSearchParams portolioSearchParams);
    Task<byte[]> GenerateGlobalLoanApplicationPdfReport(PortolioSearchParams searchParams);

    #endregion


}
