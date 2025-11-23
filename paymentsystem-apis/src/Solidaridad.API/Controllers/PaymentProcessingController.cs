using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.ExcelExport;
using Solidaridad.Application.Models.LoanApplication;
using Solidaridad.Application.Models.PaymentProcessing;
using Solidaridad.Application.Services;
using Solidaridad.Application.Services.Impl;
using Solidaridad.Core.Entities;
using Solidaridad.Core.Entities.Base;
using Solidaridad.DataAccess.Repositories;
using Solidaridad.Shared.Services;
using System;
using System.Net.Http;
using System.Security.Policy;

namespace Solidaridad.API.Controllers;

[Authorize]
public class PaymentProcessingController : ApiController
{
    #region DI
    private readonly ApiService _apiService;
    private readonly IPaymentDeductibleService _paymentDeductibleService;
    private readonly IPaymentBatchService _paymentBatchService;
    private IExcelExportService _excelExportService;
    private readonly HttpClient _httpClient;
    private readonly IPaymentBatchRepository _paymentBatchRepository;
    private readonly IClaimService _claimService;

    const string ONAFRIQ_PAYMENT_API_URL_DEV = "https://api.onafriq.com/api/v5/payments";
    const string ONAFRIQ_PAYMENT_API_URL_PROD = "https://api.onafriq.com/api/payments";


    public PaymentProcessingController(ApiService apiService,
        IExcelExportService excelExportService,
        IPaymentDeductibleService paymentDeductibleService,
        IPaymentBatchRepository paymentBatchRepository,
        IClaimService claimService,
        IPaymentBatchService paymentBatchService,
        HttpClient httpClient)
    {
        _apiService = apiService;
        _claimService = claimService;
        _paymentDeductibleService = paymentDeductibleService;
        _paymentBatchService = paymentBatchService;
        _excelExportService = excelExportService;
        _httpClient = httpClient;
        _paymentBatchRepository = paymentBatchRepository;
    }
    #endregion

    //public async Task RunApiRequests()
    //{
    //    // Example: GET request
    //    var data = await _apiService.GetAsync<dynamic>("https://api.example.com/data");

    //    // Example: POST request
    //    var result = await _apiService.PostAsync<dynamic>("https://api.example.com/data", new { Name = "John" });

    //    // Example: PUT request
    //    var updated = await _apiService.PutAsync<dynamic>("https://api.example.com/data/1", new { Name = "Doe" });

    //    // Example: DELETE request
    //    var isDeleted = await _apiService.DeleteAsync("https://api.example.com/data/1");
    //}
    #region Development builds
    [AllowAnonymous]
    [HttpPost("single")]
    public async Task InitiateSinglePayment()
    {
        var paymentRequest = new PaymentRequest
        {
            PhoneNumber = "+80000000001",
            FirstName = "John",
            LastName = "Doe",
            Amount = 100.2m, // Use decimal for monetary values
            Currency = "BXC",
            Description = "Per diem payment",
            PaymentType = "money",
            CallbackUrl = "https://my.website/payments/callback",
            Metadata = new Dictionary<string, string>
            {
                { "id", "1234" },
                { "name", "Lucy" }
            }
        };

        var result = await _apiService.PostAsync<dynamic>(ONAFRIQ_PAYMENT_API_URL_PROD, paymentRequest, customAuthToken: "ab594c14986612f6167a975e1c369e71edab6900");
    }

    [AllowAnonymous]
    [HttpPost("multiple")]
    public async Task InitiateMultiplePayment()
    {
        var recipientData = new List<Dictionary<string, string>>
        {
            new Dictionary<string, string>
            {
                { "phonenumber", "+80000000001" },
                { "first_name", "John" },
                { "last_name", "Doe" },
                { "amount", "200" },
                { "description", "Per diem payment" }
            },
            new Dictionary<string, string>
            {
                { "phonenumber", "+80000000002" },
                { "first_name", "Shan" },
                { "last_name", "Shaw" },
                { "amount", "100" },
                { "description", "Per diem payment" }
            },

        };

        // Serialize RecipientData to JSON string
        string recipientDataJson = JsonConvert.SerializeObject(recipientData);
        var paymentRequest = new MultiplePaymentRequest
        {
            Currency = "BXC",
            Description = "Per diem payment",
            PaymentType = "money",
            CallbackUrl = "https://my.website/payments/callback",
            Metadata = new Metadata
            {
                Id = "1234",
                Name = "Lucy"
            },
            RecipientData = recipientDataJson

        };

        var result = await _apiService.PostAsync<dynamic>(ONAFRIQ_PAYMENT_API_URL_PROD, paymentRequest, customAuthToken: "ab594c14986612f6167a975e1c369e71edab6900");
    }

    [AllowAnonymous]
    [HttpPost("multiple/paymentBatch")]
    public async Task InitiateMultipleBatchPayment(DeductibleExportModel searchParams)
    {
        var payments = await _excelExportService.GetDeductiblePayments(searchParams);
        if (payments == null || !payments.Any())
        {
            throw new Exception("No payment batch found");
        }

        var recipientData = new List<Dictionary<string, string>>();
        foreach (var item in payments.ToList())
        {
            recipientData.Add(
            new Dictionary<string, string>
            {
                { "phonenumber", item.Farmer.PaymentPhoneNumber },
                { "first_name", item.Farmer.FirstName },
                { "last_name", item.Farmer.OtherNames  },
                { "amount", "2" },
                { "description", "Per diem payment" }
            });
        }
        ;

        // Serialize RecipientData to JSON string
        string recipientDataJson = JsonConvert.SerializeObject(recipientData);
        var paymentRequest = new MultiplePaymentRequest
        {
            Currency = "BXC",
            Description = "Per diem payment",
            PaymentType = "money",
            CallbackUrl = "https://my.website/payments/callback",
            Metadata = new Metadata
            {
                Id = "1234",
                Name = "Lucy"
            },
            RecipientData = recipientDataJson
        };

        var result = await _apiService.PostAsync<dynamic>(ONAFRIQ_PAYMENT_API_URL_PROD, paymentRequest, customAuthToken: "ab594c14986612f6167a975e1c369e71edab6900");
    }

    [AllowAnonymous]
    [HttpGet("verifyContactsSample")]
    public async Task VerifyContactsPayment()
    {
        // Set the URL and the authorization token
        var url = "https://api.onafriq.com/api/contacts?phone_number=+80000000001&metadata={\"sync\": 1}";
        var token = "ab594c14986612f6167a975e1c369e71edab6900";

        // Add the Authorization header
        _httpClient.DefaultRequestHeaders.Clear(); // Clear any existing headers
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Token {token}");

        var result = await _apiService.GetAsync<dynamic>(url, "ab594c14986612f6167a975e1c369e71edab6900");

    }

    [AllowAnonymous]
    [HttpPost("initiateMultiplePayment")]
    public async Task InitiateMultiplePayment(DeductibleExportModel searchParams)
    {
        var payments = await _excelExportService.GetDeductiblePayments(searchParams);
        if (payments == null || !payments.Any())
        {
            throw new Exception("No payment batch found");
        }

        var recipientData = new List<Dictionary<string, string>>();
        foreach (var item in payments.ToList())
        {
            recipientData.Add(
            new Dictionary<string, string>
            {
                { "phonenumber", item.Farmer.PaymentPhoneNumber },
                { "first_name", item.Farmer.FirstName },
                { "last_name", item.Farmer.OtherNames  },
                { "amount", "2" },
                { "description", "Per diem payment" }
            });
        }
        ;

        // Serialize RecipientData to JSON string
        string recipientDataJson = JsonConvert.SerializeObject(recipientData);
        var paymentRequest = new MultiplePaymentRequest
        {
            Currency = "BXC",
            Description = "Per diem payment",
            PaymentType = "money",
            CallbackUrl = "https://my.website/payments/callback",
            Metadata = new Metadata
            {
                Id = "1234",
                Name = "Lucy"
            },
            RecipientData = recipientDataJson
        };

        var result = await _apiService.PostAsync<dynamic>(ONAFRIQ_PAYMENT_API_URL_PROD, paymentRequest, customAuthToken: "ab594c14986612f6167a975e1c369e71edab6900");
    }

    #endregion

    #region Payment and contact verification production build

    [AllowAnonymous]
    [HttpPost("verifyContacts")]
    public async Task<IActionResult> VerifyAllContacts([FromBody] DeductibleExportModel searchParams)
    {
        await _apiService.VerifyAllAsync<dynamic>(searchParams);
        return Ok(new ApiResponseModel<BaseResponseModel>
        {
            Success = true,
            Message = "success",
            Data = new BaseResponseModel { Id = searchParams.BatchId }
        });
    }

    [AllowAnonymous]
    [HttpPost("revalidateMobileNumbers")]
    public async Task<IActionResult> RevalidateMobileNumbersAsync()
    {
        await _apiService.RevalidateMobileNumbersAsync<dynamic>();
        return Ok(new ApiResponseModel<BaseResponseModel>
        {
            Success = true,
            Message = "success",
        });
    }

    [AllowAnonymous]
    [HttpPost("payAllAsync")]
    public async Task<IActionResult> PayAllAsync([FromBody] DeductibleExportModel searchParams)
    {
        await _apiService.PayAllAsync<dynamic>(searchParams);

        return Ok(new ApiResponseModel<BaseResponseModel>
        {
            Success = true,
            Message = "success",
            Data = new BaseResponseModel { Id = searchParams.BatchId }
        });
    }

    [AllowAnonymous]
    [HttpPost("payAllBatchAsync")]
    public async Task<IActionResult> PayBatchAsync([FromBody] DeductibleExportModel searchParams)
    {
        // 1. Update stage as "Approved"
        var batch = await _paymentBatchRepository.GetFirstAsync(ti => ti.Id == searchParams.BatchId);
        await _paymentBatchService.UpdateStage(batch.Id, new Application.Models.PaymentBatch.UpdateStageModel
        {
            Action = "approved",
            Remarks = "Payment submitted",
        });

        // 2. Send payments for processing
        Guid countryId = (Guid)CountryId;
        searchParams.CountryId = CountryId;
        await _apiService.PayAllBatchAsync<dynamic>(searchParams, countryId);

        return Ok(new ApiResponseModel<BaseResponseModel>
        {
            Success = true,
            Message = "success",
            Data = new BaseResponseModel { Id = searchParams.BatchId }
        });
    }

    [AllowAnonymous]
    [HttpGet("webhooks")]
    public async Task<IActionResult> GetWebHooks()
    {
        await _apiService.GetAllWebHooksAsync();
        return Ok(new ApiResponseModel<BaseResponseModel>
        {
            Success = true,
            Message = "success",
            Data = new BaseResponseModel { Id = new Guid() }
        });
    }


    [AllowAnonymous]
    [HttpPost("createhooks")]
    public async Task<IActionResult> CreateWebHooksAsync()
    {
        await _apiService.CreateWebHooksAsync();
        return Ok(new ApiResponseModel<BaseResponseModel>
        {
            Success = true,
            Message = "success",
            Data = new BaseResponseModel { Id = new Guid() }
        });
    }

    [AllowAnonymous]
    [HttpGet("getPayment")]
    public async Task<IActionResult> getPayment(int id)
    {
        var result = await _apiService.GetPaymentAsync(id);
        return Ok(new ApiResponseModel<BaseResponseModel>
        {
            Success = true,
            Message = result,
            Data = new BaseResponseModel { Id = new Guid()
            }
        });
    }

    #endregion
}
