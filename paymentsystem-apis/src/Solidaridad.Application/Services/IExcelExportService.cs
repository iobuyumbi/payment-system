using Solidaridad.Application.Models.ExcelExport;
using Solidaridad.Application.Models.ExcelImport;

namespace Solidaridad.Application.Services;

public interface IExcelExportService
{

    Task<IEnumerable<PaymentRequestDeductibleModel>> GetDeductiblePayments(DeductibleExportModel model);

    Task<PaymenReportResponseModel> GetAllDeductiblePayments();

    Task<IEnumerable<PaymentRequestFacilitationModel>> GetFacilitationPayments(Guid batchId);

    Task<IEnumerable<PaymentRequestFacilitationModel>> GetFarmers(string keyword, CancellationToken cancellationToken = default);

    Task<IEnumerable<PaymentRequestFacilitationModel>> GetApplication(string keyword, CancellationToken cancellationToken = default);
}
