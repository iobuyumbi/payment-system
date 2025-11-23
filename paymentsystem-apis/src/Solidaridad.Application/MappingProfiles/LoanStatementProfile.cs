using AutoMapper;
using Solidaridad.Application.Models.LoanRepayment;
using Solidaridad.Application.Models.PaymentBatch;
using Solidaridad.Core.Entities.Loans;
using Solidaridad.Core.Entities.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solidaridad.Application.MappingProfiles;

internal class LoanStatementProfile : Profile
{
    public LoanStatementProfile()
    {
        CreateMap<LoanStatement, LoanStatementResponseModel>();

        CreateMap<LoanStatementResponseModel, LoanStatement>();

        //CreateMap<PaymentBatch, PaymentBatchResponseModel>();

        //CreateMap<UpdatePaymentBatchModel, PaymentBatch>();

        //CreateMap<ImportPaymentBatchModel, PaymentRequestDeductible>();
    }
}
