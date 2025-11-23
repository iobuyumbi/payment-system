namespace Solidaridad.Application.Models.PaymentProcessing;

using System.Collections.Generic;
using System.Text.Json.Serialization;

public class PaymentRequest
{
    [JsonPropertyName("phonenumber")]
    public string PhoneNumber { get; set; }

    [JsonPropertyName("first_name")]
    public string FirstName { get; set; }

    [JsonPropertyName("last_name")]
    public string LastName { get; set; }

    [JsonPropertyName("amount")]
    public decimal Amount { get; set; } // Keeping it as string for consistency with PHP

    [JsonPropertyName("currency")]
    public string Currency { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("payment_type")]
    public string PaymentType { get; set; }

    [JsonPropertyName("callback_url")]
    public string CallbackUrl { get; set; }

    [JsonPropertyName("metadata")]
    public Dictionary<string, string> Metadata { get; set; }
}

public class MultiplePaymentRequest
{
    [JsonPropertyName("currency")]
    public string Currency { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("payment_type")]
    public string PaymentType { get; set; }

    [JsonPropertyName("callback_url")]
    public string CallbackUrl { get; set; }

    [JsonPropertyName("metadata")]
    public Metadata Metadata { get; set; }

    [JsonPropertyName("recipient_data")]
    public string RecipientData { get; set; }
    //public List<Dictionary<string, string>> RecipientData { get; set; }
    //public List<Recipient> RecipientData { get; set; }
}

public class Metadata
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
   
}

public class Recipient
{
    [JsonPropertyName("phonenumber")]
    public string PhoneNumber { get; set; }

    [JsonPropertyName("first_name")]
    public string FirstName { get; set; }

    [JsonPropertyName("last_name")]
    public string LastName { get; set; }

    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }
}
