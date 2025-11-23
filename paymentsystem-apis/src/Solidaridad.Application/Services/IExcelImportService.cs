using Solidaridad.Application.Models.ExcelImport;
using Solidaridad.Application.Models.PaymentBatch;
using Solidaridad.Core.Entities.Base;
using Solidaridad.Core.Entities.Excel.Import;

namespace Solidaridad.Application.Services;

public interface IExcelImportService
{
    Task<CreateExcelImportResponseModel> CreateAsync(CreateExcelImportModel createExcelImportModel, CancellationToken cancellationToken = default);

    Task<IEnumerable<ExcelImportResponseModel>> GetAllAsync(string keyword, CancellationToken cancellationToken = default);
    
    Task<IEnumerable<ExcelImportResponseModel>> Search(ImportSearchParams searchParams, CancellationToken cancellationToken = default);
    
    Task<IEnumerable<ExcelImportDetail>> GetImportDetailsAsync(Guid importId, CancellationToken cancellationToken = default);
    
    Task<UpdateExcelImportResponseModel> UpdateAsync(Guid id, UpdateExcelImportModel updateExcelImportModel, CancellationToken cancellationToken = default);
    
    Task<IEnumerable<ExcelImportDetail>> GetImportDetailsByPaymentBatch(Guid paymentBatchId, CancellationToken cancellationToken = default);
    
    Task<IEnumerable<ExcelImportResponseModel>> GetImportMainByPaymentBatch(Guid paymentBatchId, CancellationToken cancellationToken = default);
    
    Task<IEnumerable<PaymentImportResponseModel>> GetPaymentImportSummary(Guid paymentBatchId, CancellationToken cancellationToken = default);
}
