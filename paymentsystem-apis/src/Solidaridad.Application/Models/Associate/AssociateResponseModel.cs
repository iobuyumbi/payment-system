using Solidaridad.Application.Models.Farmer;
using Solidaridad.Application.Models.PaymentBatch;
using Solidaridad.Core.Entities;

namespace Solidaridad.Application.Models.Associate;

public class AssociateResponseModel
{
    public Guid FarmerId { get; set; }
    public Guid PaymentBatchId { get; set; }
    public FarmerResponseModel Farmer { get; set; }
    public PaymentBatchResponseModel PaymentBatch { get; set; }
}
