using AutoMapper;
using Solidaridad.Application.Models.ExcelImport;
using Solidaridad.Application.Models.PaymentBatch;
using Solidaridad.Application.Models.PaymentDeductible;
using Solidaridad.Core.Entities.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solidaridad.Application.MappingProfiles;

public class PaymentRequestDeductibleProfile : Profile
{
    public PaymentRequestDeductibleProfile()
    {
        CreateMap<PaymentRequestDeductible, PaymentRequestDeductibleModel>();

        CreateMap<PaymentRequestDeductibleModel, PaymentRequestDeductible>();
        CreateMap<PaymentRequestDeductible, PaymentDeductibleResponseModel>();
        CreateMap<CreatePaymentDeductibleModel, PaymentRequestDeductible>();
        CreateMap<UpdatePaymentDeductibleModel, PaymentRequestDeductible>();
        CreateMap<PaymentDeductibleResponseModel, PaymentRequestDeductible>();
        CreateMap<PaymentRequestDeductible, CreatePaymentDeductibleModel>();
        CreateMap<PaymentRequestDeductible, UpdatePaymentDeductibleModel>();
    }
}
