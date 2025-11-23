using AutoMapper;
using Solidaridad.Application.Models.PaymentBatch;
using Solidaridad.Core.Entities.Payments;

namespace Solidaridad.Application.MappingProfiles;

public class PaymentBatchProfile : Profile
{
    public PaymentBatchProfile()
    {
        CreateMap<CreatePaymentBatchModel, PaymentBatch>();

        CreateMap<PaymentBatchResponseModel, PaymentBatch>();

        CreateMap<PaymentBatch, PaymentBatchResponseModel>();

        CreateMap<UpdatePaymentBatchModel, PaymentBatch>();

         CreateMap<ImportPaymentBatchModel, PaymentRequestDeductible>();
    }
}
