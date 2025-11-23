using AutoMapper;
using LinqToDB;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using Solidaridad.Application.Models.ApiRequestLog;
using Solidaridad.Application.Models.ExcelExport;
using Solidaridad.Application.Models.ExcelImport;
using Solidaridad.Application.Models.Facilitation;
using Solidaridad.Application.Models.Farmer;
using Solidaridad.Application.Models.PaymentBatch;
using Solidaridad.Application.Models.PaymentProcessing;
using Solidaridad.Core.Entities;
using Solidaridad.Core.Entities.Loans;
using Solidaridad.Core.Entities.Payments;
using PaymentRequestDeductible = Solidaridad.Core.Entities.Payments.PaymentRequestDeductible;
using Solidaridad.DataAccess.Migrations;
using Solidaridad.DataAccess.Repositories;
using Solidaridad.DataAccess.Repositories.Impl;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using static Solidaridad.DataAccess.Persistence.Seeding.Permission.Permissions;
using static System.Net.WebRequestMethods;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Solidaridad.Application.Services.Impl;

public class ApiService
{
    #region DI
    private readonly HttpClient _httpClient;
    private readonly IApiRequestLogRepository _apiRequestLogRepository;
    private readonly IFarmerRepository _farmerRepository;
    private IExcelExportService _excelExportService;
    private readonly IMapper _mapper;
    private readonly IPaymentRequestDeductibleRepository _paymentRequestDeductibleRepository;
    private ILoanBatchRepository _loanBatchRepository;
    private ILoanRepaymentRepository _loanRepaymentRepository;
    private ILoanApplicationRepository _loanApplicationRepository;
    private IPaymenBatchLoanBatchMappingRepository _paymenBatchLoanBatchMapping;
    private readonly IFacilitationRepository _facilitationRepository;
    private IPaymentBatchService _paymentBatchService;
    private ILoanApplicationService _loanApplicationService;

    private readonly ILoanRepaymentService _loanRepaymentService;
    private readonly IPaymentBatchRepository _paymentBatchRepository;
    private readonly ICountryRepository _countryRepository;
    private readonly IPaymentBatchProjectMappingRepository _paymentBatchProjectMappingRepository;

    const string ONAFRIQ_PAYMENT_API_URL_DEV = "https://api.onafriq.com/api/v5/payments";
    const string ONAFRIQ_PAYMENT_API_URL_PROD = "https://api.onafriq.com/api/payments";
    const string ONAFRIQ_PAYMENT_TOKEN_DEV = "ab594c14986612f6167a975e1c369e71edab6900";
    const string ONAFRIQ_PAYMENT_TOKEN_PROD = "906d86b7adcbdd6b8fd9ccb69b0d374324247d17";
    const string CALLBACK_URL = "https://sdpayapi.enet.co.ke/api/CallBack";

    const string DISBURSED = "e24d24a8-fc69-4527-a92a-97f6648a43c5";

    public ApiService(HttpClient httpClient, IMapper mapper, IPaymentBatchProjectMappingRepository paymentBatchProjectMappingRepository,
        IPaymentRequestDeductibleRepository paymentRequestDeductibleRepository,
        ICountryRepository countryRepository,
        IPaymentBatchRepository paymentBatchRepository,
        IApiRequestLogRepository apiRequestLogRepository,
         ILoanBatchRepository loanBatchRepository,
         ILoanRepaymentRepository loanRepaymentRepository,
         ILoanApplicationRepository loanApplicationRepository,
         IPaymenBatchLoanBatchMappingRepository paymenBatchLoanBatchMapping,
         IExcelExportService excelExportService, IFarmerRepository farmerRepository,
         ILoanApplicationService loanApplicationService,
         IPaymentBatchService paymentBatchService,
         ILoanRepaymentService loanRepaymentService,
         IFacilitationRepository facilitationRepository)
    {
        _httpClient = httpClient;
        _apiRequestLogRepository = apiRequestLogRepository;
        _mapper = mapper;
        _excelExportService = excelExportService;
        _farmerRepository = farmerRepository;
        _paymentRequestDeductibleRepository = paymentRequestDeductibleRepository;
        _facilitationRepository = facilitationRepository;
        _loanBatchRepository = loanBatchRepository;
        _loanRepaymentRepository = loanRepaymentRepository;
        _loanApplicationRepository = loanApplicationRepository;
        _paymenBatchLoanBatchMapping = paymenBatchLoanBatchMapping;
        _paymentBatchService = paymentBatchService;
        _loanApplicationService = loanApplicationService;
        _loanRepaymentService = loanRepaymentService;
        _paymentBatchRepository = paymentBatchRepository;
        _countryRepository = countryRepository;
        _paymentBatchProjectMappingRepository = paymentBatchProjectMappingRepository;
    }
    #endregion

    #region Payment apis
    public async Task<T> GetAsync<T>(string url, string bearerToken = null)
    {
        return await ExecuteRequest<T>(url, HttpMethod.Get, null, bearerToken);
    }

    public async Task GetAllAsync<T>(List<string> urls, string bearerToken = null)
    {
        var method = HttpMethod.Get;
        List<HttpResponseMessage> responses = new List<HttpResponseMessage>();
        var apiName = urls[0].Contains("contacts", StringComparison.OrdinalIgnoreCase) ? "Contacts API" : "Payment API";
        var requestLog = new ApiRequestLog
        {
            ApiName = apiName,
            HttpMethod = method.ToString(),
            EndpointUrl = urls[0],
            RequestHeaders = JsonSerializer.Serialize(_httpClient.DefaultRequestHeaders),
            RequestBody = null,
            RequestTimestamp = DateTime.UtcNow
        };

        foreach (var url in urls)
        {
            try
            {
                if (method == HttpMethod.Get)
                {
                    _httpClient.DefaultRequestHeaders.Clear(); // Clear any existing headers
                    _httpClient.DefaultRequestHeaders.Add("Authorization", $"Token {bearerToken}");
                    responses.Add(await _httpClient.GetAsync(url));
                }
            }
            catch (Exception ex)
            {
                requestLog.IsSuccessful = false;
                requestLog.ErrorMessage = ex.Message;
                throw;
            }
        }
        var concatenatedResponseBody = new StringBuilder();

        foreach (var response in responses)
        {
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                concatenatedResponseBody.AppendLine(responseBody);
            }

        }

        // Update the request log with concatenated response body
        requestLog.StatusCode = (int)responses[0].StatusCode;
        requestLog.ResponseHeaders = JsonSerializer.Serialize(responses[0].Headers);
        requestLog.ResponseTimestamp = DateTime.UtcNow;
        requestLog.IsSuccessful = responses[0].IsSuccessStatusCode;
        requestLog.ResponseBody = concatenatedResponseBody.ToString();
        await _apiRequestLogRepository.AddAsync(requestLog);


    }

    public async Task<T> PostAsync<T>(string url, object payload, string bearerToken = null, string customAuthToken = null)
    {
        return await ExecuteRequest<T>(url, HttpMethod.Post, payload, bearerToken, customAuthToken);
    }

    public async Task<T> PutAsync<T>(string url, object payload, string bearerToken = null)
    {
        return await ExecuteRequest<T>(url, HttpMethod.Put, payload, bearerToken);
    }

    public async Task<bool> DeleteAsync(string url, string bearerToken = null)
    {
        await ExecuteRequest<object>(url, HttpMethod.Delete, null, bearerToken);
        return true;
    }

    private async Task<T> ExecuteRequest<T>(string url, HttpMethod method, object payload, string bearerToken, string customAuthToken = null)
    {
        SetAuthorizationHeader(bearerToken, customAuthToken);
        var apiName = url.Contains("contacts", StringComparison.OrdinalIgnoreCase) ? "Contacts API" : "Payment API";
        var requestLog = new ApiRequestLog
        {
            ApiName = apiName,
            HttpMethod = method.ToString(),
            EndpointUrl = url,
            RequestHeaders = JsonSerializer.Serialize(_httpClient.DefaultRequestHeaders),
            RequestBody = payload != null ? JsonSerializer.Serialize(payload) : null,
            RequestTimestamp = DateTime.UtcNow
        };

        try
        {
            HttpResponseMessage response;
            if (method == HttpMethod.Get)
            {
                _httpClient.DefaultRequestHeaders.Clear(); // Clear any existing headers
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Token {bearerToken}");
                response = await _httpClient.GetAsync(url);
            }
            else if (method == HttpMethod.Post)
            {
                var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
                response = await _httpClient.PostAsync(url, content);
            }
            else if (method == HttpMethod.Put)
            {
                var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
                response = await _httpClient.PutAsync(url, content);
            }
            else if (method == HttpMethod.Delete)
            {
                response = await _httpClient.DeleteAsync(url);
            }
            else
            {
                throw new NotSupportedException($"HTTP method {method} is not supported.");
            }

            requestLog.StatusCode = (int)response.StatusCode;
            requestLog.ResponseHeaders = JsonSerializer.Serialize(response.Headers);
            requestLog.ResponseBody = await response.Content.ReadAsStringAsync();
            requestLog.ResponseTimestamp = DateTime.UtcNow;
            requestLog.IsSuccessful = response.IsSuccessStatusCode;

            response.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<T>(requestLog.ResponseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (Exception ex)
        {
            requestLog.IsSuccessful = false;
            requestLog.ErrorMessage = ex.Message;
            throw;
        }
        finally
        {
            await _apiRequestLogRepository.AddAsync(requestLog);
        }
    }

    private void SetAuthorizationHeader(string bearerToken, string customAuthToken)
    {
        if (!string.IsNullOrEmpty(bearerToken))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
        }
        else if (!string.IsNullOrEmpty(customAuthToken))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", customAuthToken);
        }
    }


    private void SetAuthorizationHeader(string bearerToken)
    {
        if (!string.IsNullOrEmpty(bearerToken))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
        }
    }
    #endregion

    #region Get Payments log
    public async Task<string> GetSingleRequestBody(Guid id)
    {
        var _loanBatch = await _apiRequestLogRepository.GetAllAsync(c => c.Id == id);
        var loanBatch = _loanBatch.FirstOrDefault();
        return loanBatch.RequestBody;
    }
    public async Task<string> GetSingleResponseBody(Guid id)
    {
        var _loanBatch = await _apiRequestLogRepository.GetAllAsync(c => c.Id == id);
        var loanBatch = _loanBatch.FirstOrDefault();
        return loanBatch.ResponseBody;
    }

    public async Task<TransactionsResponseModel> GetAllAsync()
    {
        var _result = await _apiRequestLogRepository.GetAllAsync(c => 1 == 1);
        LogCounterResponseModel _counts = new LogCounterResponseModel
        {
            TotalTransactions = _result.Count(),
            Successful = _result.Count(c => c.IsSuccessful),
            Unsuccessful = _result.Count(c => !c.IsSuccessful),
        };



        IEnumerable<ApiRequestLogResponseModel> result = _mapper.Map<IEnumerable<ApiRequestLogResponseModel>>(_result.OrderByDescending(log => log.RequestTimestamp));

        return new TransactionsResponseModel
        {
            ApiRequestLogResponseModel = result,
            LogCounterResponseModel = _counts
        };
    }





    #endregion

    #region Payment Final flow 

    public async Task VerifyAllAsync<T>(DeductibleExportModel searchParams)
    {
        // Fetch deductible payments
        var payments = await _excelExportService.GetDeductiblePayments(searchParams);
        var farmers = await _farmerRepository.GetAllAsync(c => 1 == 1);
        List<Farmer> farmerUpdateList = new List<Farmer>();
        // Prepare recipient data
        var recipientData = payments.Select(item => new Dictionary<string, string>
    {
        { "phonenumber", item.Farmer.PaymentPhoneNumber },
        { "first_name", item.Farmer.FirstName },
        { "last_name", item.Farmer.OtherNames },
        { "amount", "2" },
        { "description", "Per diem payment" }
    }).ToList();

        // API token and endpoint
        const string token = ONAFRIQ_PAYMENT_TOKEN_PROD;


        // Configure HTTP client headers
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Token {token}");

        // Prepare API request log
        var apiName = "Contacts API";
        var requestLog = new ApiRequestLog
        {
            ApiName = apiName,
            HttpMethod = HttpMethod.Get.ToString(),
            RequestHeaders = JsonSerializer.Serialize(_httpClient.DefaultRequestHeaders),
            RequestBody = null,
            RequestTimestamp = DateTime.UtcNow
        };

        // Process each request sequentially
        var concatenatedResponseBody = new StringBuilder();
        foreach (var recipient in recipientData)
        {
            var phoneNumber = recipient["phonenumber"];
            var metadata = "{\"sync\": 1}";
            var url = $"https://api.onafriq.com/api/contacts?phone_number={phoneNumber}&metadata={metadata}";


            try
            {
                // Send the request and await its completion
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    concatenatedResponseBody.AppendLine(responseBody);

                    // Parse the JSON response
                    using var jsonDoc = JsonDocument.Parse(responseBody);
                    var root = jsonDoc.RootElement;

                    // Access the "results" array
                    if (root.TryGetProperty("results", out var resultsArray) && resultsArray.ValueKind == JsonValueKind.Array)
                    {
                        if (resultsArray.ValueKind == JsonValueKind.Array && resultsArray.GetArrayLength() > 0)
                        {
                            var result = resultsArray[0];
                            if (result.TryGetProperty("status", out var status) && status.GetString() == "active" &&
                            result.TryGetProperty("id", out var id) && id.GetInt32() != 0)
                            {

                                if (!string.IsNullOrEmpty(phoneNumber))
                                {
                                    // Fetch farmer from the 'farmers' list based on phone number
                                    var matchedFarmer = farmers.FirstOrDefault(farmer => farmer.PaymentPhoneNumber == phoneNumber);

                                    if (matchedFarmer != null)
                                    {
                                        matchedFarmer.IsFarmerVerified = true;
                                        matchedFarmer.MobileLastVerifiedOn = DateTime.UtcNow;

                                        farmerUpdateList.Add(matchedFarmer);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception and include details about the request
                await _apiRequestLogRepository.AddAsync(new ApiRequestLog
                {
                    ApiName = apiName,
                    HttpMethod = HttpMethod.Get.ToString(),
                    EndpointUrl = url,
                    RequestHeaders = JsonSerializer.Serialize(_httpClient.DefaultRequestHeaders),
                    RequestBody = null,
                    RequestTimestamp = DateTime.UtcNow,
                    IsSuccessful = false,
                    ErrorMessage = ex.Message,
                    ResponseTimestamp = DateTime.UtcNow
                });

                throw; // Re-throw to propagate failure if needed
            }
        }

        // Final logging after processing all requests
        requestLog.StatusCode = 200; // Assume 200 if all were successful (adjust if needed)
        requestLog.ResponseTimestamp = DateTime.UtcNow;
        requestLog.IsSuccessful = true;
        requestLog.ResponseBody = concatenatedResponseBody.ToString();
        await _apiRequestLogRepository.AddAsync(requestLog);
        await _farmerRepository.UpdateRange(farmerUpdateList);
    }

    public async Task RevalidateMobileNumbersAsync<T>()
    {
        // Fetch farmers
        var farmers = await _farmerRepository.GetAllAsync(c => 1 == 1);

        List<Farmer> farmerUpdateList = new List<Farmer>();

        // Prepare recipient data
        var recipientData = farmers.Select(item => new Dictionary<string, string>
        {
            { "phonenumber", item.PaymentPhoneNumber },
            { "first_name", item.FirstName },
            { "last_name", item.OtherNames },
            { "amount", "2" },
            { "description", "Per diem payment" }
        }).ToList();

        // API token and endpoint
        const string token = ONAFRIQ_PAYMENT_TOKEN_PROD;


        // Configure HTTP client headers
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Token {token}");

        // Prepare API request log
        var apiName = "Contacts API";
        var requestLog = new ApiRequestLog
        {
            ApiName = apiName,
            HttpMethod = HttpMethod.Get.ToString(),
            RequestHeaders = JsonSerializer.Serialize(_httpClient.DefaultRequestHeaders),
            RequestBody = null,
            RequestTimestamp = DateTime.UtcNow
        };

        // Process each request sequentially
        var concatenatedResponseBody = new StringBuilder();
        foreach (var recipient in recipientData)
        {
            var phoneNumber = recipient["phonenumber"];
            var metadata = "{\"sync\": 1}";
            var url = $"https://api.onafriq.com/api/contacts?phone_number={phoneNumber}&metadata={metadata}";


            try
            {
                // Send the request and await its completion
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    concatenatedResponseBody.AppendLine(responseBody);

                    // Parse the JSON response
                    using var jsonDoc = JsonDocument.Parse(responseBody);
                    var root = jsonDoc.RootElement;

                    // Access the "results" array
                    if (root.TryGetProperty("results", out var resultsArray) && resultsArray.ValueKind == JsonValueKind.Array)
                    {
                        if (resultsArray.ValueKind == JsonValueKind.Array && resultsArray.GetArrayLength() > 0)
                        {
                            var result = resultsArray[0];
                            if (result.TryGetProperty("status", out var status) && status.GetString() == "active" &&
                            result.TryGetProperty("id", out var id) && id.GetInt32() != 0)
                            {

                                if (!string.IsNullOrEmpty(phoneNumber))
                                {
                                    // Fetch farmer from the 'farmers' list based on phone number
                                    var matchedFarmer = farmers.FirstOrDefault(farmer => farmer.PaymentPhoneNumber == phoneNumber);

                                    if (matchedFarmer != null)
                                    {
                                        matchedFarmer.IsFarmerVerified = true;
                                        matchedFarmer.MobileLastVerifiedOn = DateTime.UtcNow;

                                        farmerUpdateList.Add(matchedFarmer);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception and include details about the request
                await _apiRequestLogRepository.AddAsync(new ApiRequestLog
                {
                    ApiName = apiName,
                    HttpMethod = HttpMethod.Get.ToString(),
                    EndpointUrl = url,
                    RequestHeaders = JsonSerializer.Serialize(_httpClient.DefaultRequestHeaders),
                    RequestBody = null,
                    RequestTimestamp = DateTime.UtcNow,
                    IsSuccessful = false,
                    ErrorMessage = ex.Message,
                    ResponseTimestamp = DateTime.UtcNow
                });

                throw; // Re-throw to propagate failure if needed
            }
        }

        // Final logging after processing all requests
        requestLog.StatusCode = 200; // Assume 200 if all were successful (adjust if needed)
        requestLog.ResponseTimestamp = DateTime.UtcNow;
        requestLog.IsSuccessful = true;
        requestLog.ResponseBody = concatenatedResponseBody.ToString();
        await _apiRequestLogRepository.AddAsync(requestLog);
        await _farmerRepository.UpdateRange(farmerUpdateList);
    }

    public async Task VerifyMobileAsync<T>(List<ImportPaymentBatchModel> importModel)
    {
        // Fetch deductible payments
        // var payments = await _excelExportService.GetDeductiblePayments(searchParams);
        var farmers = await _farmerRepository.GetAllAsync(c => 1 == 1);
        List<Farmer> farmerUpdateList = new List<Farmer>();

        var payments = from import in importModel
                       join farmer in farmers on import.SystemId equals farmer.SystemId
                       select new
                       {
                           import,
                           farmer
                       };

        // Prepare recipient data
        var recipientData = payments.Select(item => new Dictionary<string, string>
    {
        { "phonenumber", item.farmer.PaymentPhoneNumber },
        { "first_name", item.farmer.FirstName },
        { "last_name", item.farmer.OtherNames },
        //{ "amount", "2" },
        //{ "description", "Per diem payment" }
    }).ToList();

        // API token and endpoint
        const string token = ONAFRIQ_PAYMENT_TOKEN_PROD;

        // Configure HTTP client headers
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Token {token}");

        // Prepare API request log
        var apiName = "Contacts API";
        var requestLog = new ApiRequestLog
        {
            ApiName = apiName,
            HttpMethod = HttpMethod.Get.ToString(),
            RequestHeaders = JsonSerializer.Serialize(_httpClient.DefaultRequestHeaders),
            RequestBody = null,
            RequestTimestamp = DateTime.UtcNow
        };

        // Process each request sequentially
        var concatenatedResponseBody = new StringBuilder();
        foreach (var recipient in recipientData)
        {
            var phoneNumber = recipient["phonenumber"];
            var metadata = "{\"sync\": 1}";
            var url = $"https://api.onafriq.com/api/contacts?phone_number={phoneNumber}&metadata={metadata}";

            try
            {
                // Send the request and await its completion
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    concatenatedResponseBody.AppendLine(responseBody);

                    // Parse the JSON response
                    using var jsonDoc = JsonDocument.Parse(responseBody);
                    var root = jsonDoc.RootElement;

                    // Access the "results" array
                    if (root.TryGetProperty("results", out var resultsArray) && resultsArray.ValueKind == JsonValueKind.Array)
                    {
                        if (resultsArray.ValueKind == JsonValueKind.Array && resultsArray.GetArrayLength() > 0)
                        {
                            var result = resultsArray[0];
                            if (result.TryGetProperty("status", out var status) && status.GetString() == "active" &&
                            result.TryGetProperty("id", out var id) && id.GetInt32() != 0)
                            {

                                if (!string.IsNullOrEmpty(phoneNumber))
                                {
                                    // Fetch farmer from the 'farmers' list based on phone number
                                    var matchedFarmer = farmers.FirstOrDefault(farmer => farmer.PaymentPhoneNumber == phoneNumber);

                                    if (matchedFarmer != null)
                                    {
                                        matchedFarmer.IsFarmerVerified = true;
                                        matchedFarmer.MobileLastVerifiedOn = DateTime.UtcNow;

                                        farmerUpdateList.Add(matchedFarmer);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception and include details about the request
                await _apiRequestLogRepository.AddAsync(new ApiRequestLog
                {
                    ApiName = apiName,
                    HttpMethod = HttpMethod.Get.ToString(),
                    EndpointUrl = url,
                    RequestHeaders = JsonSerializer.Serialize(_httpClient.DefaultRequestHeaders),
                    RequestBody = null,
                    RequestTimestamp = DateTime.UtcNow,
                    IsSuccessful = false,
                    ErrorMessage = ex.Message,
                    ResponseTimestamp = DateTime.UtcNow
                });

                throw; // Re-throw to propagate failure if needed
            }
        }

        // Final logging after processing all requests
        requestLog.StatusCode = 200; // Assume 200 if all were successful (adjust if needed)
        requestLog.ResponseTimestamp = DateTime.UtcNow;
        requestLog.IsSuccessful = true;
        requestLog.ResponseBody = concatenatedResponseBody.ToString();
        await _apiRequestLogRepository.AddAsync(requestLog);
        await _farmerRepository.UpdateRange(farmerUpdateList);
    }

    public async Task PayAllAsync<T>(DeductibleExportModel searchParams)
    {
        var payments = await _excelExportService.GetDeductiblePayments(searchParams);

        const string token = ONAFRIQ_PAYMENT_TOKEN_PROD;
        var concatenatedResponseBody = new StringBuilder();
        List<PaymentRequestDeductible> deductibleUpdateList = new List<PaymentRequestDeductible>();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Token {token}");

        var requestLog = new ApiRequestLog
        {
            ApiName = "Payment API",
            HttpMethod = HttpMethod.Post.ToString(),
            RequestHeaders = JsonSerializer.Serialize(_httpClient.DefaultRequestHeaders),
            RequestBody = null,
            RequestTimestamp = DateTime.UtcNow
        };

        foreach (var item in payments.ToList())
        {
            if (item.Farmer.IsFarmerVerified && !item.IsPaymentComplete)
            {
                var paymentRequest = new PaymentRequest
                {
                    PhoneNumber = item.Farmer.PaymentPhoneNumber,
                    FirstName = item.Farmer.FirstName,
                    LastName = item.Farmer.OtherNames,
                    Amount = 11m, // Replace with the actual amount if dynamic
                    Currency = "UGX",
                    Description = "Uganda payment",
                    PaymentType = "money",
                    CallbackUrl = CALLBACK_URL,
                    Metadata = new Dictionary<string, string>
                    {
                        { "id", item.Farmer.SystemId },
                        { "name", item.Farmer.FirstName }
                    }
                };

                var url = ONAFRIQ_PAYMENT_API_URL_PROD;

                try
                {
                    // Serialize the payment request
                    var content = new StringContent(JsonSerializer.Serialize(paymentRequest), Encoding.UTF8, "application/json");

                    // Send the request
                    var response = await _httpClient.PostAsync(url, content);

                    if (response != null)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        concatenatedResponseBody.AppendLine(responseBody);
                        // Parse the JSON response
                        using var jsonDoc = JsonDocument.Parse(responseBody);
                        var root = jsonDoc.RootElement;

                        // Process the data here
                        var state = root.GetProperty("state").GetString();

                        // Access the "results" array
                        if (state == "new")
                        {
                            if (item != null)
                            {
                                item.IsPaymentComplete = true;
                                var batch = await _paymentRequestDeductibleRepository.GetFirstAsync(ti => ti.Id == item.Id);
                                _mapper.Map(item, batch);
                                deductibleUpdateList.Add(batch);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception and include details about the request
                    await _apiRequestLogRepository.AddAsync(new ApiRequestLog
                    {
                        ApiName = "Payment API",
                        HttpMethod = HttpMethod.Post.ToString(),
                        EndpointUrl = url,
                        RequestHeaders = JsonSerializer.Serialize(_httpClient.DefaultRequestHeaders),
                        RequestBody = JsonSerializer.Serialize(paymentRequest),
                        RequestTimestamp = DateTime.UtcNow,
                        IsSuccessful = false,
                        ErrorMessage = ex.Message,
                        ResponseTimestamp = DateTime.UtcNow
                    });

                    throw; // Re-throw to propagate failure if needed
                }
            }
        }

        // Final logging after processing all requests
        requestLog.StatusCode = 200;
        requestLog.ResponseTimestamp = DateTime.UtcNow;
        requestLog.IsSuccessful = true;
        requestLog.ResponseBody = concatenatedResponseBody.ToString();
        await _apiRequestLogRepository.AddAsync(requestLog);
        await _paymentRequestDeductibleRepository.UpdateRange(deductibleUpdateList);
    }

    public async Task PayAllBatchAsync<T>(DeductibleExportModel searchParams, Guid? countryId)
    {
        var paymentBatch = await _paymentBatchService.GetSingle(searchParams.BatchId, countryId);
        if (paymentBatch == null)
        {
            throw new Exception("Payment batch not found");
        }

        const string token = ONAFRIQ_PAYMENT_TOKEN_PROD;
        var concatenatedResponseBody = new StringBuilder();
        var deductibleUpdateList = new List<PaymentRequestDeductible>();
        var facilitationList = new List<PaymentRequestFacilitation>();

        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Token {token}");

        var requestLog = new ApiRequestLog
        {
            ApiName = "Payment API",
            HttpMethod = HttpMethod.Post.ToString(),
            RequestHeaders = JsonSerializer.Serialize(_httpClient.DefaultRequestHeaders),
            RequestBody = null,
            RequestTimestamp = DateTime.UtcNow
        };

        if (paymentBatch.PaymentModule == 3)
        {
            await ProcessDeductiblePayment(searchParams, concatenatedResponseBody, deductibleUpdateList, requestLog);
        }
        if (paymentBatch.PaymentModule == 4)
        {
            await ProcessFacilitationPayment(searchParams, concatenatedResponseBody, facilitationList, requestLog);
        }
        //if(paymentBatch.PaymentModule == 5)
        //{
        //    await ProcessManualPayment(searchParams, concatenatedResponseBody, deductibleUpdateList, requestLog);
        //}


        //var result = await _apiService.PostAsync<dynamic>("https://api.onafriq.com/api/v5/payments", paymentRequest, customAuthToken: "ab594c14986612f6167a975e1c369e71edab6900");
        //var response = await _httpClient.PostAsync(url, content);

        //if (response != null)
        //{
        //    var responseBody = await response.Content.ReadAsStringAsync();
        //    concatenatedResponseBody.AppendLine(responseBody);
        //    // Parse the JSON response
        //    using var jsonDoc = JsonDocument.Parse(responseBody);
        //    var root = jsonDoc.RootElement;

        //    // Process the data here
        //    var state = root.GetProperty("state").GetString();

        //    // Access the "results" array
        //    if (state == "new")
        //    {
        //        if (item != null)
        //        {
        //            item.IsPaymentComplete = true;
        //            var batch = await _paymentRequestDeductibleRepository.GetFirstAsync(ti => ti.Id == item.Id);
        //            _mapper.Map(item, batch);
        //            deductibleUpdateList.Add(batch);
        //        }
        //    }
        //}
    }

    private async Task ProcessDeductiblePayment(DeductibleExportModel searchParams, StringBuilder concatenatedResponseBody, List<PaymentRequestDeductible> deductibleUpdateList, ApiRequestLog requestLog)
    {
        var payments = await _excelExportService.GetDeductiblePayments(searchParams);
        if (payments == null || !payments.Any())
        {
            throw new Exception("No payment batch found");
        }

        var recipientData = new List<Dictionary<string, string>>();
        foreach (var item in payments.ToList())
        {
            if (item.Farmer.IsFarmerVerified && !item.IsPaymentComplete)
            {
                recipientData.Add(
                new Dictionary<string, string>
                {
                    { "phonenumber", item.Farmer.PaymentPhoneNumber },
                    { "first_name", item.Farmer.FirstName },
                    { "last_name", item.Farmer.OtherNames  },
                    { "amount", item.FarmerPayableEarningsLc.ToString() },
                    { "description", "Solidaridad Carbon Payments" }
                });
            }
        }
        ;

        // Serialize RecipientData to JSON string
        string recipientDataJson = JsonConvert.SerializeObject(recipientData);
        var paymentRequest = new MultiplePaymentRequest
        {
            Currency = payments.FirstOrDefault().Farmer.Country.Code,
            Description = "Solidaridad Carbon Payments",
            PaymentType = "money",
            CallbackUrl = CALLBACK_URL,
            Metadata = new Metadata
            {
                Id = searchParams.BatchId.ToString(),
                Name = "BatchId"
            },
            RecipientData = recipientDataJson
        };

        var url = ONAFRIQ_PAYMENT_API_URL_PROD;

        try
        {
            // Serialize the payment request
            var content = new StringContent(JsonSerializer.Serialize(paymentRequest), Encoding.UTF8, "application/json");

            // Send the request
            var response = await _httpClient.PostAsync(url, content);

            if (response != null)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                concatenatedResponseBody.AppendLine(responseBody);
                // Parse the JSON response
                using var jsonDoc = JsonDocument.Parse(responseBody);
                var root = jsonDoc.RootElement;

                // Process the data here
                var state = root.GetProperty("state").GetString();

                // Access the "results" array
                if (state == "new")
                {
                    foreach (var item in payments)
                    {
                        if (item != null && item.Farmer.IsFarmerVerified && !item.IsPaymentComplete)
                        {
                            item.IsPaymentComplete = true;
                            item.PaymentStatus = new Guid("3e3ff24a-9dd9-443c-a09c-d9c96dc36927");
                            var batch = await _paymentRequestDeductibleRepository.GetFirstAsync(ti => ti.Id == item.Id);
                            _mapper.Map(item, batch);
                            deductibleUpdateList.Add(batch);
                        }
                    }

                    // Final logging after processing all requests
                    requestLog.StatusCode = 200;
                    requestLog.ResponseTimestamp = DateTime.UtcNow;
                    requestLog.IsSuccessful = true;
                    requestLog.ResponseBody = concatenatedResponseBody.ToString();
                    await _apiRequestLogRepository.AddAsync(requestLog);
                    await _paymentRequestDeductibleRepository.UpdateRange(deductibleUpdateList);

                    // update loan payments
                    await UpdateLoanPayments(searchParams, payments, root);

                    await _paymentBatchService.UpdateStage(searchParams.BatchId, new Models.PaymentBatch.UpdateStageModel
                    {
                        Action = "approved",
                        Remarks = "Payment Completed"
                    });
                }
            }
        }
        catch (Exception ex)
        {
            // Log the exception and include details about the request
            await _apiRequestLogRepository.AddAsync(new ApiRequestLog
            {
                ApiName = "Payment API",
                HttpMethod = HttpMethod.Post.ToString(),
                EndpointUrl = url,
                RequestHeaders = JsonSerializer.Serialize(_httpClient.DefaultRequestHeaders),
                RequestBody = JsonSerializer.Serialize(paymentRequest),
                RequestTimestamp = DateTime.UtcNow,
                IsSuccessful = false,
                ErrorMessage = ex.Message,
                ResponseTimestamp = DateTime.UtcNow
            });

            throw; // Re-throw to propagate failure if needed
        }
    }

    private async Task ProcessFacilitationPayment(DeductibleExportModel searchParams, StringBuilder concatenatedResponseBody, List<PaymentRequestFacilitation> facilitationList, ApiRequestLog requestLog)
    {
        var payments = await _excelExportService.GetFacilitationPayments(searchParams.BatchId);
        if (payments == null || !payments.Any())
        {
            throw new Exception("No facilitation payment batch found");
        }
        var country = await _countryRepository.GetFirstAsync(c=> c.Id == searchParams.CountryId);

    var recipientData = new List<Dictionary<string, string>>();
        foreach (var item in payments.ToList())
        {
            if (!item.IsPaymentComplete)
            {
                string firstName = "";
                string lastName = "";

                string[] names = item.FullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (names.Length > 0)
                {
                    firstName = names[0];
                }
                if (names.Length > 1)
                {
                    lastName = names[1];
                }

                recipientData.Add(
                new Dictionary<string, string>
                {
                    { "phonenumber", item.PhoneNo },
                    { "first_name", firstName },
                    { "last_name", lastName  },
                    { "amount", item.NetDisbursementAmount.ToString() },
                    { "description", "Solidaridad Facilitations Payments" }
                });
            }
        }
        ;
    
        // Serialize RecipientData to JSON string
        string recipientDataJson = JsonConvert.SerializeObject(recipientData);
        var paymentRequest = new MultiplePaymentRequest
        {
            Currency = country.Code,
            Description = "Solidaridad Facilitations Payments",
            PaymentType = "money",
            CallbackUrl = CALLBACK_URL,
            Metadata = new Metadata
            {
                Id = searchParams.BatchId.ToString(),
                Name = "BatchId"
            },
            RecipientData = recipientDataJson
        };

        var url = ONAFRIQ_PAYMENT_API_URL_PROD;

        try
        {
            // Serialize the payment request
            var content = new StringContent(JsonSerializer.Serialize(paymentRequest), Encoding.UTF8, "application/json");

            // Send the request
            var response = await _httpClient.PostAsync(url, content);

            if (response != null)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                concatenatedResponseBody.AppendLine(responseBody);
                // Parse the JSON response
                using var jsonDoc = JsonDocument.Parse(responseBody);
                var root = jsonDoc.RootElement;

                // Process the data here
                var state = root.GetProperty("state").GetString();

                // Access the "results" array
                if (state == "new")
                {
                    string refNumber = "NA";
                    //necessary evil // look for a better way
                    if (root.ValueKind != JsonValueKind.Undefined && root.TryGetProperty("id", out JsonElement idElement))
                    {
                        refNumber = idElement.GetRawText(); // or .GetInt32().ToString() if always an integer
                        var batch = await _paymentBatchRepository.GetFirstAsync(x => x.Id == searchParams.BatchId);
                        batch.ReferenceNumber = refNumber;
                        await _paymentBatchRepository.UpdateAsync(batch);
                    }
                   
                    foreach (var item in payments)
                    {
                        if (item != null && !item.IsPaymentComplete)
                        {
                            item.IsPaymentComplete = true;
                            item.PaymentStatus = new Guid("3e3ff24a-9dd9-443c-a09c-d9c96dc36927");
                            var batch = await _facilitationRepository.GetFirstAsync(ti => ti.Id == item.Id);
                            _mapper.Map(item, batch);
                            facilitationList.Add(batch);
                        }
                    }

                    // Final logging after processing all requests
                    requestLog.StatusCode = 200;
                    requestLog.ResponseTimestamp = DateTime.UtcNow;
                    requestLog.IsSuccessful = true;
                    requestLog.ResponseBody = concatenatedResponseBody.ToString();
                    await _apiRequestLogRepository.AddAsync(requestLog);
                    await _facilitationRepository.UpdateRange(facilitationList);


                    await _paymentBatchService.UpdateStage(searchParams.BatchId, new Models.PaymentBatch.UpdateStageModel
                    {
                        Action = "approved",
                        Remarks = "Payment Completed"
                    });
                }
            }
        }
        catch (Exception ex)
        {
            // Log the exception and include details about the request
            await _apiRequestLogRepository.AddAsync(new ApiRequestLog
            {
                ApiName = "Payment API",
                HttpMethod = HttpMethod.Post.ToString(),
                EndpointUrl = url,
                RequestHeaders = JsonSerializer.Serialize(_httpClient.DefaultRequestHeaders),
                RequestBody = JsonSerializer.Serialize(paymentRequest),
                RequestTimestamp = DateTime.UtcNow,
                IsSuccessful = false,
                ErrorMessage = ex.Message,
                ResponseTimestamp = DateTime.UtcNow
            });

            throw; // Re-throw to propagate failure if needed
        }
    }






    private async Task ProcessManualPayment(DeductibleExportModel searchParams, StringBuilder concatenatedResponseBody, List<PaymentRequestDeductible> deductibleUpdateList, ApiRequestLog requestLog)
    {
        var payments = await _excelExportService.GetDeductiblePayments(searchParams);
        if (payments == null || !payments.Any())
        {
            throw new Exception("No payment batch found");
        }
        try
        {
                    foreach (var item in payments)
                    {
                        if (item != null && item.Farmer.IsFarmerVerified && !item.IsPaymentComplete)
                        {
                            item.IsPaymentComplete = true;
                            item.IsManual = true;
                            item.PaymentStatus = new Guid("271d9c1a-2c4f-4ee2-ad0f-d7dc36bd255f");
                            var batch = await _paymentRequestDeductibleRepository.GetFirstAsync(ti => ti.Id == item.Id);
                            _mapper.Map(item, batch);
                            deductibleUpdateList.Add(batch);
                        }
                    }

                   
                    await _paymentRequestDeductibleRepository.UpdateRange(deductibleUpdateList);

            // update loan payments
            JsonElement root = JsonDocument.Parse("{}").RootElement;
            await UpdateLoanPayments(searchParams, payments, root);

                    await _paymentBatchService.UpdateStage(searchParams.BatchId, new Models.PaymentBatch.UpdateStageModel
                    {
                        Action = "approved",
                        Remarks = "Payment Completed"
                    });
                
            
        }
        catch (Exception ex)
        {
           
            throw ex; 
        }
    }








    public async Task UpdateLoanPayments(DeductibleExportModel searchParams, IEnumerable<PaymentRequestDeductibleModel> payments, JsonElement root)
    {
        var paymenBatchLoanBatchMapping = await _paymenBatchLoanBatchMapping.GetAllAsync(x => x.PaymentBatchId == searchParams.BatchId);
      
        var paymentBatch = new PaymentBatch();
        if (paymenBatchLoanBatchMapping != null)
        {
            var loanBatch = await _loanBatchRepository.GetAllAsync(x =>true);
            if (paymenBatchLoanBatchMapping.Any())
            {
                loanBatch = await _loanBatchRepository.GetAllAsync(x => x.Id == paymenBatchLoanBatchMapping.FirstOrDefault().LoanBatchId);
            }
           
            paymentBatch = await _paymentBatchRepository.GetFirstAsync(ti => ti.Id == searchParams.BatchId
        && ti.CountryId == searchParams.CountryId
        && ti.IsDeleted == false);

            var loanApplication = await _loanApplicationRepository.GetAllAsync(x=> x.Status == Guid.Parse(DISBURSED));

            foreach (var item in payments)
            {
                if (item.IsPaymentComplete == true)
                {
                    try
                    {
                        Guid loanApplicationId = Guid.NewGuid();
                        var matchingLoanApplication = new LoanApplication();

                        if (loanApplication != null)
                        {
                            matchingLoanApplication = loanApplication?.FirstOrDefault(app => app.LoanNumber == item.LoanAccountNo);
                            if (matchingLoanApplication != null)
                            {
                                loanApplicationId = matchingLoanApplication.Id;
                            }
                            else
                            {
                                continue;
                                //throw new InvalidOperationException($"No matching LoanApplication found for item with SystemId: {item.SystemId}");
                            }
                        }
                        else
                        {
                            continue;
                        }

                        string refNumber = "NA";

                        if (root.ValueKind != JsonValueKind.Undefined && root.TryGetProperty("id", out JsonElement idElement))
                        {
                            refNumber = idElement.GetRawText(); // or .GetInt32().ToString() if always an integer
                        }


                        /* newer method*/
                        _loanRepaymentRepository.ApplyPayment(
                           loanApplicationId,
                           item.FarmerLoansDeductionsLc,
                           paymentBatch != null ? paymentBatch.BatchName : "NA",
                          refNumber
                           );

                        if (paymentBatch != null)
                        {
                            paymentBatch.ReferenceNumber = refNumber;
                        }
                        await _paymentBatchRepository.UpdateAsync(paymentBatch);
                        // To generate the latest payment based loan statement
                        await _loanRepaymentService.GenerateLatestPaymentBasedLoanStatement(loanApplicationId, item.FarmerEarningsShareLc, refNumber);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }
    }


    public async Task CreateContactAsync(CreateFarmerModel model)
    {
        var metadataJson = JsonSerializer.Serialize(new { my_id = model.SystemId });
        var data = new Dictionary<string, string>
    {
        { "phone_number", model.PaymentPhoneNumber },
        { "first_name", model.FirstName },
        { "last_name", model.OtherNames },
        { "email", model.Email },
        { "metadata[my_id]", metadataJson }
    };

        // API token and endpoint
        const string token = ONAFRIQ_PAYMENT_TOKEN_PROD;

        // Configure HTTP client headers
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Token {token}");

        // Prepare API request log
        var apiName = "Create Contacts API";
        var requestLog = new ApiRequestLog
        {
            ApiName = apiName,
            HttpMethod = HttpMethod.Get.ToString(),
            RequestHeaders = JsonSerializer.Serialize(_httpClient.DefaultRequestHeaders),
            RequestBody = JsonSerializer.Serialize(data),
            RequestTimestamp = DateTime.UtcNow
        };

        // Process each request sequentially
        var concatenatedResponseBody = new StringBuilder();
        var url = $"https://api.onafriq.com/api/contacts";

        try
        {
            var content = new FormUrlEncodedContent(data);
            // Send the request and await its completion
            var response = await _httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                concatenatedResponseBody.AppendLine(responseBody);

                // Parse the JSON response
                using var jsonDoc = JsonDocument.Parse(responseBody);
                var root = jsonDoc.RootElement;

                // Access the "results" array
                if (root.TryGetProperty("results", out var resultsArray) && resultsArray.ValueKind == JsonValueKind.Array)
                {
                    if (resultsArray.ValueKind == JsonValueKind.Array && resultsArray.GetArrayLength() > 0)
                    {

                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Log the exception and include details about the request
            await _apiRequestLogRepository.AddAsync(new ApiRequestLog
            {
                ApiName = apiName,
                HttpMethod = HttpMethod.Get.ToString(),
                EndpointUrl = url,
                RequestHeaders = JsonSerializer.Serialize(_httpClient.DefaultRequestHeaders),
                RequestBody = null,
                RequestTimestamp = DateTime.UtcNow,
                IsSuccessful = false,
                ErrorMessage = ex.Message,
                ResponseTimestamp = DateTime.UtcNow
            });

            throw; // Re-throw to propagate failure if needed
        }


        // Final logging after processing all requests
        requestLog.StatusCode = 200; // Assume 200 if all were successful (adjust if needed)
        requestLog.ResponseTimestamp = DateTime.UtcNow;
        requestLog.IsSuccessful = true;
        requestLog.ResponseBody = concatenatedResponseBody.ToString();
        await _apiRequestLogRepository.AddAsync(requestLog);

    }
    public async Task CreateImportContactAsync(Farmer model)
    {
        var metadataJson = JsonSerializer.Serialize(new { my_id = model.SystemId });
        var data = new Dictionary<string, string>
    {
        { "phone_number", model.PaymentPhoneNumber },
        { "first_name", model.FirstName },
        { "last_name", model.OtherNames },
        { "email", model.Email },
        { "metadata[my_id]", metadataJson }
    };

        // API token and endpoint
        const string token = ONAFRIQ_PAYMENT_TOKEN_PROD;

        // Configure HTTP client headers
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Token {token}");

        // Prepare API request log
        var apiName = "Create Contacts API";
        var requestLog = new ApiRequestLog
        {
            ApiName = apiName,
            HttpMethod = HttpMethod.Get.ToString(),
            RequestHeaders = JsonSerializer.Serialize(_httpClient.DefaultRequestHeaders),
            RequestBody = JsonSerializer.Serialize(data),
            RequestTimestamp = DateTime.UtcNow
        };

        // Process each request sequentially
        var concatenatedResponseBody = new StringBuilder();
        var url = $"https://api.onafriq.com/api/contacts";

        try
        {
            var content = new FormUrlEncodedContent(data);
            // Send the request and await its completion
            var response = await _httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                concatenatedResponseBody.AppendLine(responseBody);

                // Parse the JSON response
                using var jsonDoc = JsonDocument.Parse(responseBody);
                var root = jsonDoc.RootElement;

                // Access the "results" array
                if (root.TryGetProperty("results", out var resultsArray) && resultsArray.ValueKind == JsonValueKind.Array)
                {
                    if (resultsArray.ValueKind == JsonValueKind.Array && resultsArray.GetArrayLength() > 0)
                    {

                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Log the exception and include details about the request
            await _apiRequestLogRepository.AddAsync(new ApiRequestLog
            {
                ApiName = apiName,
                HttpMethod = HttpMethod.Get.ToString(),
                EndpointUrl = url,
                RequestHeaders = JsonSerializer.Serialize(_httpClient.DefaultRequestHeaders),
                RequestBody = null,
                RequestTimestamp = DateTime.UtcNow,
                IsSuccessful = false,
                ErrorMessage = ex.Message,
                ResponseTimestamp = DateTime.UtcNow
            });

            throw; // Re-throw to propagate failure if needed
        }


        // Final logging after processing all requests
        requestLog.StatusCode = 200; // Assume 200 if all were successful (adjust if needed)
        requestLog.ResponseTimestamp = DateTime.UtcNow;
        requestLog.IsSuccessful = true;
        requestLog.ResponseBody = concatenatedResponseBody.ToString();
        await _apiRequestLogRepository.AddAsync(requestLog);

    }


    public async Task<string> GetAllContactAsync()
    {
        // API token and endpoint
        const string token = ONAFRIQ_PAYMENT_TOKEN_PROD;
        var url = "https://api.onafriq.com/api/contacts";

        // Configure HTTP client headers
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Token {token}");

        // Prepare API request log
        var apiName = "Get Contacts API";
        var requestLog = new ApiRequestLog
        {
            ApiName = apiName,
            HttpMethod = HttpMethod.Get.ToString(),
            EndpointUrl = url,
            RequestHeaders = JsonSerializer.Serialize(_httpClient.DefaultRequestHeaders),
            RequestBody = null, // No body in GET request
            RequestTimestamp = DateTime.UtcNow
        };

        try
        {
            // Send the GET request
            var response = await _httpClient.GetAsync(url);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                // Parse the JSON response
                using var jsonDoc = JsonDocument.Parse(responseBody);
                var root = jsonDoc.RootElement;

                // Extract results array
                if (root.TryGetProperty("results", out var resultsArray) && resultsArray.ValueKind == JsonValueKind.Array)
                {
                    //return resultsArray.ToString();
                }
            }

            // Log the response
            requestLog.StatusCode = (int)response.StatusCode;
            requestLog.ResponseBody = responseBody;
            requestLog.IsSuccessful = response.IsSuccessStatusCode;
            requestLog.ResponseTimestamp = DateTime.UtcNow;
        }
        catch (Exception ex)
        {
            // Log the exception
            requestLog.IsSuccessful = false;
            requestLog.ErrorMessage = ex.Message;
            requestLog.ResponseTimestamp = DateTime.UtcNow;

            throw; // Rethrow to propagate the error
        }
        finally
        {
            // Save request log
            await _apiRequestLogRepository.AddAsync(requestLog);
        }

        return null;
    }

    public async Task<string> GetAllWebHooksAsync()
    {
        // API token and endpoint
        const string token = ONAFRIQ_PAYMENT_TOKEN_PROD;
        var url = "https://api.onafriq.com/api/webhooks";

        // Configure HTTP client headers
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Token {token}");

        // Prepare API request log
        var apiName = "Get Webhooks API";
        var requestLog = new ApiRequestLog
        {
            ApiName = apiName,
            HttpMethod = HttpMethod.Get.ToString(),
            EndpointUrl = url,
            RequestHeaders = JsonSerializer.Serialize(_httpClient.DefaultRequestHeaders),
            RequestBody = null, // No body in GET request
            RequestTimestamp = DateTime.UtcNow
        };

        try
        {
            // Send the GET request
            var response = await _httpClient.GetAsync(url);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                // Parse the JSON response
                using var jsonDoc = JsonDocument.Parse(responseBody);
                var root = jsonDoc.RootElement;

                // Extract results array
                if (root.TryGetProperty("results", out var resultsArray) && resultsArray.ValueKind == JsonValueKind.Array)
                {
                    //return resultsArray.ToString(); // Convert the array to a string
                }
            }

            // Log the response
            requestLog.StatusCode = (int)response.StatusCode;
            requestLog.ResponseBody = responseBody;
            requestLog.IsSuccessful = response.IsSuccessStatusCode;
            requestLog.ResponseTimestamp = DateTime.UtcNow;
        }
        catch (Exception ex)
        {
            // Log the exception
            requestLog.IsSuccessful = false;
            requestLog.ErrorMessage = ex.Message;
            requestLog.ResponseTimestamp = DateTime.UtcNow;

            throw; // Rethrow to propagate the error
        }
        finally
        {
            // Save request log
            await _apiRequestLogRepository.AddAsync(requestLog);
        }

        return null;
    }

    public async Task<string> CreateWebHooksAsync()
    {
        // API token and endpoint
        const string token = ONAFRIQ_PAYMENT_TOKEN_PROD;
        var url = "https://api.onafriq.com/api/webhooks";
        var data = new Dictionary<string, string>
    {
        { "event","payment.status.changed" },
        { "target", CALLBACK_URL},

    };
        // Configure HTTP client headers
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Token {token}");

        // Prepare API request log
        var apiName = "Webhooks API";
        var requestLog = new ApiRequestLog
        {
            ApiName = apiName,
            HttpMethod = HttpMethod.Get.ToString(),
            EndpointUrl = url,
            RequestHeaders = JsonSerializer.Serialize(_httpClient.DefaultRequestHeaders),
            RequestBody = JsonSerializer.Serialize(data),
            RequestTimestamp = DateTime.UtcNow
        };
        var jsonContent = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
        try
        {
            // Send the GET request
            var response = await _httpClient.PostAsync(url, jsonContent);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                // Parse the JSON response
                using var jsonDoc = JsonDocument.Parse(responseBody);
                var root = jsonDoc.RootElement;

                // Extract results array
                if (root.TryGetProperty("results", out var resultsArray) && resultsArray.ValueKind == JsonValueKind.Array)
                {
                    return resultsArray.ToString();
                }
            }

            // Log the response
            requestLog.StatusCode = (int)response.StatusCode;
            requestLog.ResponseBody = responseBody;
            requestLog.IsSuccessful = response.IsSuccessStatusCode;
            requestLog.ResponseTimestamp = DateTime.UtcNow;
        }
        catch (Exception ex)
        {
            // Log the exception
            requestLog.IsSuccessful = false;
            requestLog.ErrorMessage = ex.Message;
            requestLog.ResponseTimestamp = DateTime.UtcNow;

            throw; // Rethrow to propagate the error
        }
        finally
        {
            // Save request log
            await _apiRequestLogRepository.AddAsync(requestLog);
        }

        return null;
    }




    #endregion

    #region Fetch Payments


    public async Task<string> GetPaymentAsync(int paymentId)
    {
        const string token = ONAFRIQ_PAYMENT_TOKEN_PROD;
        var url = $"https://api.onafriq.com/api/v5/payments/{paymentId}";

        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Token {token}");
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

        try
        {
            var response = await _httpClient.GetAsync(url);

            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"GET failed with status code {response.StatusCode}. Response: {responseBody}");
            }

            return responseBody;
        }
        catch (Exception ex)
        {
            // You can log the error here if needed
            throw new Exception($"Error retrieving payment from Onafriq: {ex.Message}", ex);
        }
    }



    #endregion
}

