using AutoMapper;
using Solidaridad.Application.Models.ExcelImport;
using Solidaridad.Application.Models.PaymentBatch;
using Solidaridad.Core.Entities.Base;
using Solidaridad.Core.Entities.Excel.Import;
using Solidaridad.Core.Entities.Payments;
using Solidaridad.DataAccess.Repositories;
using Solidaridad.Shared.Services;
using System.Globalization;
using ExcelImport = Solidaridad.Core.Entities.ExcelImport;

namespace Solidaridad.Application.Services.Impl;

public class ExcelImportService : IExcelImportService
{
    #region DI

    private readonly IExcelImportRepository _excelImportRepository;
    private readonly IExcelImportDetailRepository _excelImportDetailRepository;
    private readonly IPaymentRequestDeductibleRepository _paymentRequestDeductibleRepository;
    private readonly IClaimService _claimService;
    private readonly IMapper _mapper;

    public ExcelImportService(IExcelImportRepository excelImportRepository,
        IExcelImportDetailRepository excelImportDetailRepository,
        IPaymentRequestDeductibleRepository paymentRequestDeductibleRepository,
        IClaimService claimService,
        IMapper mapper)
    {
        _excelImportRepository = excelImportRepository;
        _excelImportDetailRepository = excelImportDetailRepository;
        _paymentRequestDeductibleRepository = paymentRequestDeductibleRepository;
        _claimService = claimService;
        _mapper = mapper;
    }

    #endregion

    #region Methods

    public async Task<CreateExcelImportResponseModel> CreateAsync(CreateExcelImportModel createExcelImportModel, CancellationToken cancellationToken = default)
    {
        try
        {
            var excelImport = _mapper.Map<ExcelImport>(createExcelImportModel);
            var addedExcelImport = await _excelImportRepository.AddAsync(excelImport);

            return new CreateExcelImportResponseModel
            {
                Id = addedExcelImport.Id
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<IEnumerable<ExcelImportResponseModel>> GetAllAsync(string keyword, CancellationToken cancellationToken = default)
    {
        var excelImports = await _excelImportRepository.GetAllAsync(tl => 1 == 1);

        if (!string.IsNullOrEmpty(keyword))
        {
            excelImports = excelImports.Where(e =>
                         CultureInfo.CurrentCulture.CompareInfo.IndexOf(e.Filename, keyword, CompareOptions.IgnoreCase) >= 0
                    ).ToList();
        }

        return _mapper.Map<IEnumerable<ExcelImportResponseModel>>(excelImports);
    }

    public async Task<UpdateExcelImportResponseModel> UpdateAsync(Guid id, UpdateExcelImportModel updateExcelImportModel, CancellationToken cancellationToken = default)
    {
        var excelImport = await _excelImportRepository.GetFirstAsync(tl => tl.Id == id);

        excelImport.EndDateTime = updateExcelImportModel.EndDateTime;
        excelImport.ExcelImportStatusID = updateExcelImportModel.ExcelImportStatusID;
        excelImport.StatusRemark = updateExcelImportModel.StatusRemarks;

        return new UpdateExcelImportResponseModel
        {
            Id = (await _excelImportRepository.UpdateAsync(excelImport)).Id
        };
    }

    #endregion

    public async Task<IEnumerable<ExcelImportDetail>> GetImportDetailsAsync(Guid importId, CancellationToken cancellationToken = default)
    {
        var excelImports = await _excelImportDetailRepository.GetAllAsync(c => c.ExcelImportId == importId);

        return _mapper.Map<IEnumerable<ExcelImportDetail>>(excelImports);
    }

    public async Task<IEnumerable<ExcelImportResponseModel>> Search(ImportSearchParams searchParams, CancellationToken cancellationToken = default)
    {
        var imports = await _excelImportRepository.GetAllAsync(c => c.PaymentBatchId == searchParams.PaymentBatchId && c.CountryId == searchParams.CountryId);

        int numberOfObjectsPerPage = searchParams.PageSize;

        var queryResultPage = imports
            .Skip(numberOfObjectsPerPage * (searchParams.PageNumber - 1))
            .Take(numberOfObjectsPerPage);

        return _mapper.Map<IEnumerable<ExcelImportResponseModel>>(queryResultPage.OrderBy(c => c.ImportedDateTime));
    }

    public async Task<IEnumerable<ExcelImportDetail>> GetImportDetailsByPaymentBatch(Guid paymentBatchId, CancellationToken cancellationToken = default)
    {
        var excelImport = await _excelImportRepository.GetAllAsync(c => c.PaymentBatchId == paymentBatchId);
        if (excelImport == null)
        {
            throw new Exception("Payment Batch not found");
        }
        var excelImports = await _excelImportDetailRepository.GetAllAsync(c => c.ExcelImportId == excelImport.LastOrDefault().Id);

        return _mapper.Map<IEnumerable<ExcelImportDetail>>(excelImports);
    }

    public async Task<IEnumerable<ExcelImportResponseModel>> GetImportMainByPaymentBatch(Guid paymentBatchId, CancellationToken cancellationToken = default)
    {
        var excelImports = await _excelImportRepository.GetAllAsync(c => c.PaymentBatchId == paymentBatchId);

        return _mapper.Map<IEnumerable<ExcelImportResponseModel>>(excelImports);
    }

    public async Task<IEnumerable<PaymentImportResponseModel>> GetPaymentImportSummary(Guid paymentBatchId, CancellationToken cancellationToken = default)
    {
        var excelImports = await _excelImportRepository.GetAllAsync(c => c.PaymentBatchId == paymentBatchId);
        if (excelImports == null)
        {
            throw new Exception("Payment Batch not found");
        }
        var list = new List<PaymentImportResponseModel>();
        foreach (var excelImport in excelImports)
        {
            var imports = await _paymentRequestDeductibleRepository.GetPaymentImportSummary(c => c.ExcelImportId == excelImport.Id);
            foreach (var importer in imports)
            {
                list.Add(new PaymentImportResponseModel
                {
                    ExcelImportId = excelImport.Id,
                    FailedRows = importer.FailedRows,
                    ImportDate = excelImport.ImportedDateTime,
                    PassedRows = importer.PassedRows,
                    TotalRowsInExcel = importer.TotalRowsInExcel
                });
            }
        }
        return list;
    }
}
