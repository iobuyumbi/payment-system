using AutoMapper;
using FluentValidation;
using LinqToDB.Common.Internal.Cache;
using Microsoft.Extensions.Caching.Memory;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.Country;
using Solidaridad.Application.Models.County;
using Solidaridad.Application.Models.Farmer;
using Solidaridad.Application.Models.Validators.AdminLevel1;
using Solidaridad.Application.Models.Validators.Farmer;
using Solidaridad.Core.Entities;
using Solidaridad.Core.Entities.Base;
using Solidaridad.DataAccess.Repositories;
using System.Collections.ObjectModel;

namespace Solidaridad.Application.Services.Impl;

public class AdminLevel1Service : IAdminLevel1Service
{

    #region DI
    private readonly IMapper _mapper;
    private readonly IAdminLevel1Repository _countyRepository;
    private readonly ICountryRepository _countryRepository;
    private const string _memoryCacheKeyCountries = "DB-Counties";
    private readonly IMemoryCache _memoryCache;
    private readonly CreateAdminLevel1Validator _createAdminLevel1Validator;
    private readonly UpdateAdminLevel1Validator _updateAdminLevel1Validator;

    public AdminLevel1Service(IMapper mapper,
          IMemoryCache memoryCache,
          IAdminLevel1Repository countyRepository,
          ICountryRepository countryRepository)
    {
        _mapper = mapper;
        _countyRepository = countyRepository;
        _memoryCache = memoryCache;
        _createAdminLevel1Validator = new CreateAdminLevel1Validator();
        _updateAdminLevel1Validator = new UpdateAdminLevel1Validator();
        _countryRepository = countryRepository;
    }
    #endregion

    #region Methods
    public async Task<ReadOnlyCollection<AdminLevel1ResponseModel>> GetAllAsync(AdminLevel1SearchParams searchParams)
    {

        var _counties = new List<AdminLevel1>();
        var countries = await _countryRepository.GetAllAsync(c => c.Id == searchParams.CountryId);

        if (searchParams.CountryId != null)
        {
            _counties = await _countyRepository.GetAllAsync(c => (c.CountryId == searchParams.CountryId.Value)
            && (string.IsNullOrEmpty(searchParams.Filter) ||
            c.CountyName.Contains(searchParams.Filter) ||
            c.CountyCode.Contains(searchParams.Filter)));
        }
        else
        {
            _counties = await _countyRepository.GetAllAsync(c => (string.IsNullOrEmpty(searchParams.Filter) ||
            c.CountyName.Contains(searchParams.Filter) ||
            c.CountyCode.Contains(searchParams.Filter)));
        }

        var counties = _mapper.Map<ReadOnlyCollection<AdminLevel1ResponseModel>>(_counties);
        foreach (var item in counties)
        {
            item.CountryName = countries.FirstOrDefault(c => c.Id == item.CountryId).CountryName;
        }

        return counties;
    }

    public async Task<CreateAdminLevel1ResponseModel> CreateAsync(CreateAdminLevel1Model createCountyModel)
    {
        try
        {
            var validationResult = _createAdminLevel1Validator.Validate(createCountyModel);
            if (!validationResult.IsValid)
            {

                throw new ValidationException("Validation failed", validationResult.Errors);
            }
            var county = _mapper.Map<AdminLevel1>(createCountyModel);
            var addedCountry = await _countyRepository.AddAsync(county);

            return new CreateAdminLevel1ResponseModel
            {
                Id = addedCountry.Id
            };
        }
        catch (ValidationException ex)
        {
            throw ex;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<BaseResponseModel> DeleteAsync(Guid id)
    {
        var county = await _countyRepository.GetFirstAsync(tl => tl.Id == id);

        return new BaseResponseModel
        {
            Id = (await _countyRepository.DeleteAsync(county)).Id
        };
    }

    public async Task<UpdateAdminLevel1ResponseModel> UpdateAsync(Guid id, UpdateAdminLevel1Model updateCountyModel)
    {
        try
        {
            var validationResult = _updateAdminLevel1Validator.Validate(updateCountyModel);
            if (!validationResult.IsValid)
            {

                throw new ValidationException("Validation failed", validationResult.Errors);
            }
            var _county = await _countyRepository.GetAllAsync(ti => ti.Id == id);
            var county = _county.FirstOrDefault();
            _mapper.Map(updateCountyModel, county);

            return new UpdateAdminLevel1ResponseModel
            {
                Id = (await _countyRepository.UpdateAsync(county)).Id
            };
        }
        catch (ValidationException ex)
        {
            throw ex;
        }
        catch (Exception ex) {throw ex; }

    }
    #endregion
}
