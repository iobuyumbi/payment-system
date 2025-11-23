using Solidaridad.Core.Common;

namespace Solidaridad.Application.Models.CallBack;

//public class CreateCallBackModel
//{
//    public CreateHookModel Hook { get; set; } = new CreateHookModel();
//    public CreateCallBackRecordsModel Data { get; set; } = new CreateCallBackRecordsModel();
//}
//public class CreateCallBackRecordsModel 
//{
//    public int Id { get; set; }
//    public int Organization { get; set; }
//    public string Amount { get; set; }
//    public string Currency { get; set; }
//    public string Payment_type { get; set; }
//    public MetadataModel Metadata { get; set; } = new MetadataModel();
//    public string Description { get; set; }
//    public List<string> Phone_nos { get; set; }
//    public string State { get; set; }
//    public string? LastError { get; set; }
//    public string? RejectedReason { get; set; }
//    public int? RejectedBy { get; set; }
//    public DateTime? RejectedTime { get; set; }
//    public string? CancelledReason { get; set; }
//    public int? CancelledBy { get; set; }
//    public DateTime? CancelledTime { get; set; }
//    public DateTime Created { get; set; }
//    public int Author { get; set; }
//    public DateTime Modified { get; set; }
//    public int? UpdatedBy { get; set; }
//    public DateTime StartDate { get; set; }
//}


//public class CreateHookModel
//{
//    public int Id { get; set; }
//    public DateTime Created { get; set; }
//    public DateTime Updated { get; set; }
//    public string Event { get; set; } = string.Empty;
//    public string Target { get; set; } = string.Empty;
//    public int User { get; set; }
//}

//public class CreateCallBackResponseModel : BaseResponseModel
//{

//}


public class CreateCallBackModel
{
    public CreateHookModel Hook { get; set; } = new CreateHookModel();
    public CreateCallBackRecordsModel Data { get; set; } = new CreateCallBackRecordsModel();
    public int Event { get; set; }
}

public class CreateCallBackRecordsModel
{
    public int Id { get; set; }
    public int Organization { get; set; }
    public string Amount { get; set; } = string.Empty;
    public bool AmountInSendCurrency { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string Payment_type { get; set; } = string.Empty;
    public MetadataModel Metadata { get; set; } = new MetadataModel();
    public string Description { get; set; } = string.Empty;
    public List<PhoneNoModel> Phone_nos { get; set; } = new List<PhoneNoModel>();
    public string State { get; set; } = string.Empty;
    public string? LastError { get; set; }
    public string? RejectedReason { get; set; }
    public int? RejectedBy { get; set; }
    public DateTime? RejectedTime { get; set; }
    public string? CancelledReason { get; set; }
    public int? CancelledBy { get; set; }
    public DateTime? CancelledTime { get; set; }
    public decimal ChargedFee { get; set; }
    public DateTime Created { get; set; }
    public int Author { get; set; }
    public DateTime Modified { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime StartDate { get; set; }
    public bool SendSmsMessage { get; set; }
    public string? RemoteTransactionId { get; set; }
    public string? RequestCurrency { get; set; }
    public List<PaymentFxDetailsModel> Payment_fx_details { get; set; } = new List<PaymentFxDetailsModel>();
    public List<TransactionModel> Transactions { get; set; } = new List<TransactionModel>();
}



public class PhoneNoModel
{
    public string Amount { get; set; } = string.Empty;
    public int BatchId { get; set; }
    public string Contact { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Last_error { get; set; }
    public string? Metadata { get; set; }
    public string? PausedReason { get; set; }
    public int PaymentId { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
}

public class PaymentFxDetailsModel
{
    public int PaymentId { get; set; }
    public decimal ReceiveAmount { get; set; }
    public string ReceiveCurrency { get; set; } = string.Empty;
    public decimal ReceiveFxRate { get; set; }
    public string RequestAmount { get; set; } = string.Empty;
    public string RequestCurrency { get; set; } = string.Empty;
    public decimal RequestFxRate { get; set; }
    public string SendAmount { get; set; } = string.Empty;
    public string SendCurrency { get; set; } = string.Empty;
    public decimal SendFxRate { get; set; }
}

public class TransactionModel
{
    // Define transaction properties if needed (JSON showed an empty array)
}

public class CreateHookModel
{
    public int Id { get; set; }
    public string Event { get; set; } = string.Empty;
    public string Target { get; set; } = string.Empty;
}

public class CreateCallBackResponseModel : BaseResponseModel
{
}
