using AutoMapper;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.CallBack;
using Solidaridad.Core.Entities;
using Solidaridad.DataAccess.Repositories;
using Solidaridad.DataAccess.Repositories.Impl;
using System.Text.Json;
using System;

namespace Solidaridad.Application.Services.Impl;

public class CallBackService : ICallBackService
{
    #region DI
    private readonly IMapper _mapper;
    private readonly ICallBackRepository _callBackRepository;
    private readonly IPaymentBatchRepository _paymentBatchRepository;
    private readonly IPaymentRequestDeductibleRepository _paymentRequestDeductibleRepository; 
    private readonly IFacilitationRepository _facilitationRepository;
    private readonly IFarmerRepository _farmerRepository;
    private readonly IPaymentDeductibleStatusMasterRepository _paymentDeductibleStatusMasterRepository;
    public CallBackService(IMapper mapper, ICallBackRepository callBackRepository ,
        IPaymentDeductibleStatusMasterRepository paymentDeductibleStatusMasterRepository,
        IPaymentRequestDeductibleRepository paymentRequestDeductibleRepository,
        IFacilitationRepository facilitationRepository,
        IFarmerRepository farmerRepository,
        IPaymentBatchRepository paymentBatchRepository)
    {
        
        _mapper = mapper;
        _callBackRepository = callBackRepository;
        _paymentBatchRepository = paymentBatchRepository;
        _paymentRequestDeductibleRepository = paymentRequestDeductibleRepository;
        _farmerRepository = farmerRepository;
        _facilitationRepository = facilitationRepository;
        _paymentDeductibleStatusMasterRepository = paymentDeductibleStatusMasterRepository;

    }
    #endregion 
    #region Methods
    public async Task<CreateCallBackResponseModel> CreateAsync(CreateCallBackModel createCallBackModel)
    {
        try
        {

            CallBackRecords model = new CallBackRecords
            {
                Organization = createCallBackModel.Data.Organization,
                Amount = createCallBackModel.Data.Amount,
                Currency = createCallBackModel.Data.Currency,
                PaymentType = createCallBackModel.Data.Payment_type,
                Metadata = JsonSerializer.Serialize(createCallBackModel.Data.Metadata, new JsonSerializerOptions { WriteIndented = false }),
                Description = createCallBackModel.Data.Description,
                PhoneNos = JsonSerializer.Serialize(createCallBackModel.Data.Phone_nos, new JsonSerializerOptions { WriteIndented = false }),
                State = createCallBackModel.Data.State,
                LastError = createCallBackModel.Data.LastError,
                RejectedReason = createCallBackModel.Data.RejectedReason,
                RejectedBy = createCallBackModel.Data.RejectedBy,
                RejectedTime = createCallBackModel.Data.RejectedTime,
                CancelledReason = createCallBackModel.Data.CancelledReason,
                CancelledBy = createCallBackModel.Data.CancelledBy,
                CancelledTime = createCallBackModel.Data.CancelledTime,
                Created = createCallBackModel.Data.Created,
                Author = createCallBackModel.Data.Author,
                Modified = createCallBackModel.Data.Modified,
                UpdatedBy = createCallBackModel.Data.UpdatedBy,
                StartDate = createCallBackModel.Data.StartDate,
                ReferenceId = Convert.ToString(createCallBackModel.Data.Id),
                CallBackResponse = Convert.ToString(createCallBackModel)
            };

           string result = await HandleOnafriqCallbackAsync(createCallBackModel);

            model.Id = Guid.NewGuid();
            var addedCooperative = await _callBackRepository.AddAsync(model);

            return new CreateCallBackResponseModel
            {
                Id = addedCooperative.Id
            };
        }
     
        catch (Exception ex)
        {
            throw ex;
        }

    }

    public async Task<string> HandleOnafriqCallbackAsync(CreateCallBackModel callback)
    {
        var referenceNumber = callback.Data.Id.ToString();
        var batchId = Guid.Parse(callback.Data.Metadata.Id);
        var statusMaster = await _paymentDeductibleStatusMasterRepository.GetAllAsync(x => true);
        // Step 1: Get the matching Payment Batch using ReferenceNumber
        var paymentBatch = await _paymentBatchRepository.GetFirstAsync(x => x.ReferenceNumber == referenceNumber || x.Id == batchId);
        if (paymentBatch == null)
        {
            return $"Payment batch not found for ReferenceNumber {referenceNumber}";
        }

        // Step 2: Get all payments in the batch
        if (paymentBatch.PaymentModule == 3)
        {
            var payments = await _paymentRequestDeductibleRepository.GetAllAsync(x => x.PaymentBatchId == paymentBatch.Id);
            if (payments == null || !payments.Any())
            {
                return $"No payments found under batch {paymentBatch.Id}";
            }

            // Step 3: Match phone numbers from callback and update payment records
            foreach (var phoneCallback in callback.Data.Phone_nos)
            {
                var farmer = await _farmerRepository.GetFirstAsync(f => f.PaymentPhoneNumber == phoneCallback.PhoneNumber);
                var matchingPayment = payments.FirstOrDefault(p =>
                    p.SystemId == farmer.SystemId
                );

                if (matchingPayment != null)
                {

                    var newStatus = MapOnafriqStatusToInternal(phoneCallback.State);
                    string status = statusMaster.FirstOrDefault(x => x.Id == matchingPayment.PaymentStatus)?.Name ?? "Unknown";

                    if (IsValidTransition(status, phoneCallback.State))
                    {
                        matchingPayment.PaymentStatus = newStatus;
                        matchingPayment.Remarks = phoneCallback.Last_error ?? phoneCallback.PausedReason ?? "";
                        //matchingPayment.UpdatedOn = DateTime.UtcNow;
                    }
                }
            }

            // Step 4: Save updated payments
            await _paymentRequestDeductibleRepository.UpdateRange(payments);
            return $"Successfully updated {payments.Count} payments for batch {paymentBatch.Id}";
        }
        else
        {
            var payments = await _facilitationRepository.GetAllAsync(x => x.PaymentBatchId == paymentBatch.Id);
            if (payments == null || !payments.Any())
            {
                return $"No payments found under batch {paymentBatch.Id}";
            }
            // Step 3: Match phone numbers from callback and update payment records
            foreach (var phoneCallback in callback.Data.Phone_nos)
            {
                var matchingPayment = payments.FirstOrDefault(p =>
                    p.PhoneNo == phoneCallback.PhoneNumber
                );
                if (matchingPayment != null)
                {

                    var newStatus = MapOnafriqStatusToInternal(phoneCallback.State);
                    string status = statusMaster.FirstOrDefault(x => x.Id == matchingPayment.PaymentStatus)?.Name ?? "Unknown";
                    if (IsValidTransition(status, phoneCallback.State))
                    {
                        matchingPayment.PaymentStatus = newStatus;
                        matchingPayment.Remarks = phoneCallback.Last_error ?? phoneCallback.PausedReason ?? "";
                       //matchingPayment.UpdatedOn = DateTime.UtcNow;
                    }
                    
                }
            }
            // Step 4: Save updated payments
            await _facilitationRepository.UpdateRange(payments);

            return $"Successfully updated {payments.Count} payments for batch {paymentBatch.Id}";
        }

    }

  




    private Guid MapOnafriqStatusToInternal(string onafriqState)
    {
        return onafriqState.ToLower() switch
        {
            "new" => Guid.Parse("3e3ff24a-9dd9-443c-a09c-d9c96dc36927"),
            "processing" => Guid.Parse("27b6555b-ab7a-4189-a437-ae124bc8e6e7"),
            "pending_confirmation" => Guid.Parse("68682d11-ed34-4fb7-bae0-825dff8cceb9"),
            "complete" => Guid.Parse("271d9c1a-2c4f-4ee2-ad0f-d7dc36bd255f"),
            "error" => Guid.Parse("573fbb1a-5213-4264-bccc-dc2f530f2761"),
            "paused" => Guid.Parse("8c481cbc-6dad-4b42-aef5-6c9990d34740"),
            "parked" => Guid.Parse("b156ba98-7091-4236-9bfd-199050acfc24"),
            "paused_for_admin_action" => Guid.Parse("10467bda-86c0-46a6-bbec-498ee85d3823"),
            "queued" => Guid.Parse("edc0c3a0-ac71-4c7e-8fc9-e0d6a551b652"),
            "aborted" => Guid.Parse("58a0686f-0d27-48e0-8940-55a26b3601f4"),
            "created" => Guid.Parse("d8a75d19-0b59-4ba0-95a4-f800e48da2c9"),
            "cancelled" => Guid.Parse("7a21a61d-c0a0-4231-837f-54682f3a27c0"), 
            "scheduled" => Guid.Parse("1aac93f4-645c-4d92-808b-19b36216b7b1"), 
            "processed_with_errors" => Guid.Parse("c91f291e-6583-472b-9efd-02b66d07157a"), 
            _ => Guid.Empty
        };
    }

    public async Task<BaseResponseModel> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<CallBackResponseModel>> GetAllAsync()
    {
        var _cooperative = await _callBackRepository.GetAllAsync(c => true);
        

        return _mapper.Map<IEnumerable<CallBackResponseModel>>(_cooperative);
    }
    #endregion


    #region helpers
    private static readonly HashSet<string> NewStates = new() { "new" };
    private static readonly HashSet<string> IntermediaryStates = new()
{
    "processing", "paused", "parked", "paused_for_admin_action", "queued", "pending_confirmation"
};
    private static readonly HashSet<string> FinalStates = new()
{
    "complete", "aborted", "error"
};

    private bool IsValidTransition(string current, string next)
    {
        if (string.IsNullOrEmpty(current)) return true; // First assignment allowed

        if (NewStates.Contains(current))
        {
            return IntermediaryStates.Contains(next) || FinalStates.Contains(next);
        }
        if (IntermediaryStates.Contains(current))
        {
            return IntermediaryStates.Contains(next) || FinalStates.Contains(next);
        }
        if (FinalStates.Contains(current))
        {
            return false; // final → anything not allowed
        }

        return true;
    }
    #endregion
}
