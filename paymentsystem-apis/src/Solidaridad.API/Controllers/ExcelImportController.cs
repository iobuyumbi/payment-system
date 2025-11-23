using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.ExcelImport;
using Solidaridad.Application.Models.PaymentBatch;
using Solidaridad.Application.Services;
using Solidaridad.Core.Entities.Base;
using Solidaridad.Core.Entities.Excel.Import;
using Solidaridad.Core.Entities.Pagination;
using Solidaridad.Core.Enums;

namespace Solidaridad.API.Controllers;

[Authorize]
public class ExcelImportController : ApiController
{
    #region DI
    private IExcelImportService _excelImportService;
    private IFarmerService _farmerService;
    private ILoanApplicationService _loanApplicationService;
    private IPaymentService _paymentService;
    private IPermissionService _permissionService;

    public ExcelImportController(IExcelImportService excelImportService, IPaymentService paymentService, IFarmerService farmerService,
        IPermissionService permissionService,
        ILoanApplicationService loanApplicationService)
    {
        _excelImportService = excelImportService;
        _farmerService = farmerService;
        _loanApplicationService = loanApplicationService;
        _paymentService = paymentService;
        _permissionService = permissionService;

    }
    #endregion

    #region Public Methods

    [AllowAnonymous]
    [HttpPost("ImportData/{module}/{countryId}/{paymentBatchId?}")]
    public async Task<IActionResult> ImportData(IFormFile file, ImportModuleEnum module, string countryId, Guid? paymentBatchId)
    {
        try
        {
            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var spaceName = "sdpay";
            var region = "nyc3"; // DigitalOcean Spaces region

            var client = new AmazonS3Client(
                "DO006YKDC9UKCZUN7KA8",
                "e1cECatyV6rgPp5hH2M+Qb5UErNlmXR4RBBMXpLUjNI",
                new AmazonS3Config
                {
                    ServiceURL = "https://sdpay.nyc3.digitaloceanspaces.com",
                    ForcePathStyle = true
                });

            using var stream = file.OpenReadStream();

            var request = new PutObjectRequest
            {
                BucketName = spaceName,
                Key = fileName,
                InputStream = stream,
                ContentType = file.ContentType,
                CannedACL = S3CannedACL.PublicRead // or Private if you prefer
            };

            var response = await client.PutObjectAsync(request);
            string fileUrl = $"https://{spaceName}.{region}.digitaloceanspaces.com/{fileName}";

            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {

            }

            #region Save Excel Import
            var createModel = new CreateExcelImportModel
            {
                Filename = file.FileName,
                ImportedDateTime = DateTime.UtcNow,
                ExcelImportStatusID = (int)ImportStatus.Started,
                Module = module.ToString(),
                PaymentBatchId = paymentBatchId,
                CountryId = Guid.Parse(countryId),
                BlobFolder = fileUrl
            };

            if (file == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "File not found.");
            }

            var createResult = await _excelImportService.CreateAsync(createModel);
            if (createResult == null && !createResult.Id.Equals(Guid.Empty))
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to save Excel import.");
            }
            #endregion

            #region Import Data
            switch (module)
            {
                case ImportModuleEnum.Farmer:
                    await _farmerService.ImportFarmer(file, createResult.Id);
                    break;
                case ImportModuleEnum.LoanApplication:
                    await _loanApplicationService.ImportLoanApplication(file, createResult.Id, paymentBatchId);
                    break;
                case ImportModuleEnum.PaymentDeductibles:
                    await _paymentService.ImportPaymentRequestDeductibleMultiBatch(file, createResult.Id, (Guid)paymentBatchId);
                    break;
                case ImportModuleEnum.PaymentFacilitations:
                    await _paymentService.ImportPaymentRequestFacilitation(file, createResult.Id, (Guid)paymentBatchId);
                    break;
                case ImportModuleEnum.Peremission:
                    await _permissionService.ImportPermission(file, createResult.Id);
                    break;
            }
            #endregion

            #region Update Excel Import
            var updateModel = new UpdateExcelImportModel
            {
                EndDateTime = DateTime.UtcNow,
                ExcelImportStatusID = (int)ImportStatus.Completed
            };

            var final_result = await _excelImportService.UpdateAsync(createResult.Id.Value, updateModel);
            #endregion

            if (final_result.Id.Equals(Guid.Empty))
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to update Excel import.");
            }

            return Ok(ApiResult<ExcelImportResponseModel>.Success(new
                    ExcelImportResponseModel
            {
                Id = createResult.Id,
                Filename = createModel.Filename,
            }));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, string.IsNullOrEmpty(ex.Message) ? "An error occurred while processing your request." : ex.Message);
        }
    }

    [HttpPost]
    [Route("Search")]
    public async Task<IActionResult> Search(ImportSearchParams searchParams)
    {
        searchParams.CountryId = CountryId;
        var excelImports = await _excelImportService.Search(searchParams);

        int totalRecords = excelImports.Count();
        Page pageInfo = new Page
        {
            PageNumber = searchParams.PageNumber,
            Size = searchParams.PageSize,
            TotalElements = totalRecords,
            TotalPages = totalRecords / searchParams.PageSize
        };
        var pagedData = new PagedData<List<ExcelImportResponseModel>>
        {
            Page = pageInfo,
            Result = excelImports.ToList()
        };

        return Ok(new ApiResponseModel<PagedData<List<ExcelImportResponseModel>>>
        {
            Success = true,
            Message = "success",
            Data = pagedData
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(string keyword)
    {
        var imports = await _excelImportService.GetAllAsync(keyword);
        imports = imports.Where(c => c.CountryId == CountryId);

        return Ok(ApiResult<IEnumerable<ExcelImportResponseModel>>.Success(imports.OrderByDescending(c => c.ImportedDateTime)));
    }

    [HttpGet("detail/{id:guid}")]
    public async Task<IActionResult> GetDetail(Guid id)
    {
        var imports = await _excelImportService.GetImportDetailsAsync(id);

        return Ok(ApiResult<IEnumerable<ExcelImportDetail>>.Success(imports.OrderBy(c => c.Id)));
    }

    [HttpGet("payment-batch/{paymentBatchId:guid}/import-details")]
    public async Task<IActionResult> GetImportDetailsByPaymentBatch(Guid paymentBatchId)
    {
        var details = await _excelImportService.GetImportDetailsByPaymentBatch(paymentBatchId);

        return Ok(ApiResult<IEnumerable<ExcelImportDetail>>.Success(details.OrderBy(c => c.Id)));
    }

    [HttpGet("payment-batch/{paymentBatchId:guid}/import-summary")]
    public async Task<IActionResult> PaymentImportSummary(Guid paymentBatchId)
    {
        var details = await _excelImportService.GetPaymentImportSummary(paymentBatchId);

        return Ok(ApiResult<IEnumerable<PaymentImportResponseModel>>.Success(details.OrderByDescending(c => c.ImportDate)));
    }
    #endregion
}
