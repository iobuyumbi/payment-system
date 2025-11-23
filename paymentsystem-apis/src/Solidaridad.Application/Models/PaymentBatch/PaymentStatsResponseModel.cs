namespace Solidaridad.Application.Models.PaymentBatch;


public class PaymentStatsResponseModel
    {
        public int TotalPaymentBatches { get; set; }
        public Dictionary<string, int> StageCounts { get; set; }
    }

