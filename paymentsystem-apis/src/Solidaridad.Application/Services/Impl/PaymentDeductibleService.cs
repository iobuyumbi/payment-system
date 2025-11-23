using AutoMapper;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.ExcelImport;
using Solidaridad.Application.Models.Farmer;
using Solidaridad.Application.Models.PaymentDeductible;
using Solidaridad.Core.Entities.Base;
using Solidaridad.Core.Entities.Payments;
using Solidaridad.DataAccess.Repositories;
using Solidaridad.DataAccess.Repositories.Impl;
using System.Globalization;
namespace Solidaridad.Application.Services.Impl;

public class PaymentDeductibleService : IPaymentDeductibleService
{
    #region DI
    private readonly IMapper _mapper;
    private readonly IPaymentRequestDeductibleRepository _paymentRequestDeductibleRepository;
    private readonly IFacilitationRepository _facilitationRepository;
    private readonly IFarmerRepository _farmerRepository;

    public PaymentDeductibleService(IMapper mapper, IFarmerRepository farmerRepository,
        IFacilitationRepository facilitationRepository,
        IPaymentRequestDeductibleRepository paymentRequestDeductibleRepository)
    {
        _mapper = mapper;
        _paymentRequestDeductibleRepository = paymentRequestDeductibleRepository;
        _farmerRepository = farmerRepository;
        _facilitationRepository = facilitationRepository;

    }
    #endregion

    #region Methods
    public async Task<CreatePaymentDeductibleResponseModel> CreateAsync(CreatePaymentDeductibleModel model)
    {
        try
        {
            var _model = _mapper.Map<PaymentRequestDeductible>(model);
            _model.PaymentStatus = new Guid("d8a75d19-0b59-4ba0-95a4-f800e48da2c9");
            var addedBatch = await _paymentRequestDeductibleRepository.AddAsync(_model);

            return new CreatePaymentDeductibleResponseModel
            {
                Id = addedBatch.Id,

            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<BaseResponseModel> DeleteAsync(Guid id)
    {
        var batch = await _paymentRequestDeductibleRepository.GetFirstAsync(tl => tl.Id == id);
        batch.IsDeleted = true;
        return new BaseResponseModel
        {
            Id = (await _paymentRequestDeductibleRepository.UpdateAsync(batch)).Id
        };
    }

    public async Task<IEnumerable<PaymentDeductibleResponseModel>> GetAllAsync(DeductibleSearchParams searchParams)
    {
        var _payments = await _paymentRequestDeductibleRepository.GetAllAsync(c => c.PaymentBatchId == searchParams.PaymentBatchId &&
        c.IsDeleted == false);
        var farmers = await _farmerRepository.GetAllAsync(c => true);
        var payments = _mapper.Map<IEnumerable<PaymentDeductibleResponseModel>>(_payments);
        foreach (var payment in payments)
        {
            payment.NationalId = farmers.FirstOrDefault(c => c.SystemId == payment.SystemId).BeneficiaryId;
        }
        return payments;
    }

    public async Task<IEnumerable<PaymentRequestDeductibleModel>> GetAllForReportAsync()
    {
        var paymentList = await _paymentRequestDeductibleRepository.GetAllAsync(
            ti =>
          ti.IsDeleted == false
        );

        var list = _mapper.Map<IEnumerable<PaymentRequestDeductibleModel>>(paymentList);
        var farmers = await _farmerRepository.GetAllAsync(c => 1 == 1);

        //if (model.StatusId == 1)
        //{
        foreach (var payment in list)
        {
            // Fetch a single farmer or null if no match
            var _farmer = farmers.FirstOrDefault(c => c.SystemId == payment.SystemId);


            payment.Farmer = _farmer != null
                ? _mapper.Map<FarmerResponseModel>(_farmer)
                : null; // Assign an empty object if no match found
        }
        //}
        var finalList = list.Where(p => p.Farmer != null).ToList();

        return finalList;
    }

    public async Task<PaymentDeductibleResponseModel> GetByIdAsync(Guid id)
    {
        var paymentBatch = await _paymentRequestDeductibleRepository.GetFirstAsync(ti => ti.Id == id);
        var _paymentBatch = _mapper.Map<PaymentDeductibleResponseModel>(paymentBatch);
        return _paymentBatch;
    }

    public async Task<UpdatePaymentDeductibleResponseModel> UpdateAsync(Guid id, UpdatePaymentDeductibleModel model)
    {
        var batch = await _paymentRequestDeductibleRepository.GetFirstAsync(ti => ti.Id == id);
        _mapper.Map(model, batch);
        var updatedbatch = await _paymentRequestDeductibleRepository.UpdateAsync(batch);
        return new UpdatePaymentDeductibleResponseModel
        {
            Id = id
        };
    }

    public async Task<PaymentStats> GetPaymentBatchStats(Guid paymentBatchId)
    {
        var _payments = await _paymentRequestDeductibleRepository.GetAllAsync(c => c.PaymentBatchId == paymentBatchId && c.IsDeleted == false);

        var count = _payments.Count();
        var sum = _payments.Sum(c => (c.FarmerPayableEarningsLc));

        return new PaymentStats { BeneficiaryCount = count, TotalAmount = sum };
    }

    public async Task<PaymentStats> GetFacilitationPaymentBatchStats(Guid paymentBatchId)
    {
        var _payments = await _facilitationRepository.GetAllAsync(c => c.PaymentBatchId == paymentBatchId && c.IsDeleted == false);

        var count = _payments.Count();
        var sum = _payments.Sum(c => (c.NetDisbursementAmount));

        return new PaymentStats { BeneficiaryCount = count, TotalAmount = (decimal)sum };
    }


    public async Task<IEnumerable<PaymentSummaryResponseModel>> GetAllPaymentsData()
    {
        var currentDate = DateTime.UtcNow;
        var startDate = currentDate.AddYears(-1).AddMonths(1);

        var payments = await _paymentRequestDeductibleRepository.GetAllAsync(
            c => c.CreatedOn >= startDate && c.CreatedOn < currentDate && c.IsDeleted == false
        ) ?? new List<PaymentRequestDeductible>();

        var groupedPayments = payments
            .GroupBy(p => new { p.CreatedOn.Year, p.CreatedOn.Month })
            .Select(g => new PaymentSummaryResponseModel
            {
                Year = g.Key.Year,
                Month = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(g.Key.Month),
                Total = g.Sum(p => p.FarmerEarningsShareLc)
            })
            .OrderBy(p => new DateTime(p.Year, DateTime.ParseExact(p.Month, "MMM", CultureInfo.CurrentCulture).Month, 1))
            .ToList();

        return groupedPayments;
    }



    #endregion
}
