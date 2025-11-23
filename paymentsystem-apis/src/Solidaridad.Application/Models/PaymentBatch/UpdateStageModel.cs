namespace Solidaridad.Application.Models.PaymentBatch;

public class UpdateStageModel
{
    public string Action { get; set; }

    public string Remarks { get; set; }

     public Guid PerformedBy { get; set; } 
}
