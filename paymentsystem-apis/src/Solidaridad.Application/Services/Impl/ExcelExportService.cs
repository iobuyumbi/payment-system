using AutoMapper;
using Solidaridad.Application.Models.Country;
using Solidaridad.Application.Models.ExcelExport;
using Solidaridad.Application.Models.ExcelImport;
using Solidaridad.Application.Models.Farmer;
using Solidaridad.Application.Models.LoanApplication;
using Solidaridad.Core.Entities.Base;
using Solidaridad.DataAccess.Repositories;
using Solidaridad.DataAccess.Repositories.Impl;
using Solidaridad.Shared.Services;

namespace Solidaridad.Application.Services.Impl;

public class ExcelExportService : IExcelExportService
{
    #region DI


    private readonly IMapper _mapper;
    private readonly IPaymentRequestDeductibleRepository _paymentRequestDeductibleRepository;
    private readonly IFacilitationRepository _facilitationRepository;
    private readonly IFarmerRepository _farmerRepository;
    private readonly ICountryRepository _countryRepository;
    public ExcelExportService(
        IPaymentRequestDeductibleRepository paymentRequestDeductibleRepository, IFarmerRepository farmerRepository, ICountryRepository countryRepository,
    IMapper mapper, IFacilitationRepository facilitationRepository)
    {

        _paymentRequestDeductibleRepository = paymentRequestDeductibleRepository;

        _mapper = mapper;
        _farmerRepository = farmerRepository;
        _countryRepository = countryRepository;
        _facilitationRepository = facilitationRepository;   
    }

    public async Task<PaymenReportResponseModel> GetAllDeductiblePayments()
    {
        var paymentList = await _paymentRequestDeductibleRepository.GetAllAsync(
          ti =>
          ti.StatusId >= 0
      );

        var list = _mapper.Map<IEnumerable<PaymentRequestDeductibleModel>>(paymentList);
        var farmers = await _farmerRepository.GetAllAsync(c => 1 == 1);
        var _countries = await _countryRepository.GetAllAsync(c => c.IsActive == true);
        //if (model.StatusId == 1)
        //{
        foreach (var payment in list)
        {
            // Fetch a single farmer or null if no match
            var _farmer = farmers.FirstOrDefault(c => c.SystemId == payment.SystemId);

            if (_farmer != null)
            {
                payment.Farmer = _mapper.Map<FarmerResponseModel>(_farmer); // Assign an empty object if no match found

                payment.Farmer.Country = _mapper.Map<CountryResponseModel>(_countries.FirstOrDefault(c => c.Id == _farmer?.CountryId));
            }

        }
        //}
        var finalList = list.Where(p => p.Farmer != null).ToList();
        finalList.ForEach(c =>
        {

            c.Farmer.ValidationSource = "onafriq";
        });
        PaymentDeductibleStatsModel stats = new PaymentDeductibleStatsModel
        {
            TotalAmount = (float)finalList.Sum(c => c.FarmerEarningsShareLc),
            TotalBeneficiaries = finalList.Count(c => c.Farmer.IsFarmerVerified),
            TotalLoanDeductions = (float)finalList.Sum(c => c.FarmerLoansDeductionsLc),
            TotalPaymentCost = 0,
            FailedTransactions = finalList.Count(c => !c.IsPaymentComplete),
            SuccessfulTransactions = finalList.Count(c => c.IsPaymentComplete),
            TotalPayment = finalList.Count(),

        };


        return new PaymenReportResponseModel
        {
            PaymentRequestDeductibleModels = finalList,
            PaymentDeductibleStats = stats
        };
    }

    #endregion

    #region Methods
    public Task<IEnumerable<PaymentRequestFacilitationModel>> GetApplication(string keyword, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<PaymentRequestDeductibleModel>> GetDeductiblePayments(DeductibleExportModel model)
    {
        var paymentList = await _paymentRequestDeductibleRepository.GetAllAsync(
            ti =>
            ti.StatusId >= 0 &&
            ti.PaymentBatchId == model.BatchId && ti.IsDeleted == false
        );

        var list = _mapper.Map<IEnumerable<PaymentRequestDeductibleModel>>(paymentList);
        var farmers = await _farmerRepository.GetAllAsync(c => 1 == 1);
        var _countries = await _countryRepository.GetAllAsync(c => c.IsActive == true);
        //if (model.StatusId == 1)
        //{
        foreach (var payment in list)
        {
            // Fetch a single farmer or null if no match
            var _farmer = farmers.FirstOrDefault(c => c.SystemId == payment.SystemId);

            if (model.IsFarmerValid.HasValue)
            {
                _farmer = farmers.FirstOrDefault(c => c.SystemId == payment.SystemId);
            }
            if (_farmer != null)
            {
                payment.Farmer = _mapper.Map<FarmerResponseModel>(_farmer); // Assign an empty object if no match found

                payment.Farmer.Country = _mapper.Map<CountryResponseModel>(_countries.FirstOrDefault(c => c.Id == _farmer?.CountryId));
                payment.NationalId = _farmer.BeneficiaryId;
            }

        }
        //}
        var finalList = list.Where(p => p.Farmer != null).ToList();
        finalList.ForEach(c =>
        {

            c.Farmer.ValidationSource = "onafriq";
        });

        return finalList;
    }

    public async Task<IEnumerable<PaymentRequestFacilitationModel>> GetFacilitationPayments(Guid batchId)
    {
        var paymentList = await _facilitationRepository.GetAllAsync(
             ti =>
             ti.StatusId >= 0 &&
             ti.PaymentBatchId == batchId
         );

        var list = _mapper.Map<IEnumerable<PaymentRequestFacilitationModel>>(paymentList);

        return list;
    }

    public Task<IEnumerable<PaymentRequestFacilitationModel>> GetFarmers(string keyword, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
    #endregion
}
