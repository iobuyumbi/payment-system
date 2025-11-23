using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.AdminLevels;
using Solidaridad.Application.Models.Country;
using Solidaridad.Application.Models.County;
using Solidaridad.Application.Models.SubCounty;
using Solidaridad.Application.Models.Village;
using Solidaridad.Application.Models.Ward;
using Solidaridad.Core.Entities;
using Solidaridad.DataAccess.Repositories;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Solidaridad.Application.Services.Impl;

public class CountryService : ICountryService
{
    #region DI
    private readonly IMapper _mapper;
    private readonly ICountryRepository _countryRepository;
    private const string _memoryCacheKeyCountries = "DB-Countries";
    private readonly IMemoryCache _memoryCache;
    private readonly IAdminLevel1Repository _adminLevel1Repository;
    private readonly IAdminLevel2Repository _adminLevel2Repository;
    private readonly IAdminLevel3Repository _adminLevel3Repository;
    private readonly IAdminLevel4Repository _adminLevel4Repository;

    public CountryService(IMapper mapper,
          IMemoryCache memoryCache,
          ICountryRepository countryRepository,
           IAdminLevel1Repository adminLevel1Repository,
          IAdminLevel2Repository adminLevel2Repository,
          IAdminLevel3Repository adminLevel3Repository,
         IAdminLevel4Repository adminLevel4Repository)
    {
        _mapper = mapper;
        _countryRepository = countryRepository;
        _memoryCache = memoryCache;
        _adminLevel1Repository = adminLevel1Repository;
        _adminLevel4Repository = adminLevel4Repository;
        _adminLevel2Repository = adminLevel2Repository;
        _adminLevel3Repository = adminLevel3Repository;
    }
    #endregion

    #region Methods
    public async Task<ReadOnlyCollection<CountryResponseModel>> GetAllWithCountryIdAsync(Guid? countryId)
    {
        if (!_memoryCache.TryGetValue<ReadOnlyCollection<CountryResponseModel>>(
            _memoryCacheKeyCountries,
            out ReadOnlyCollection<CountryResponseModel> countries) || (countries == null || !countries.Any()))
        {
            var _countries = new List<Country>();
            if (countryId != Guid.Empty)
            {
                _countries = await _countryRepository.GetAllAsync(c => countryId == c.Id);
            }
            else
            {
                _countries = await _countryRepository.GetAllAsync(c => 1 == 1);
            }

            if (_countries != null && _countries.Any())
            {
                countries = _mapper.Map<ReadOnlyCollection<CountryResponseModel>>(_countries);
                _memoryCache.Set(_memoryCacheKeyCountries, countries.OrderBy(c => c.CountryName), new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.Date.AddDays(1).AddTicks(-1)
                });
            }
        }

        return countries;
    }
    public async Task<ReadOnlyCollection<CountryResponseModel>> GetAllAsync()
    {
        if (!_memoryCache.TryGetValue<ReadOnlyCollection<CountryResponseModel>>(
            _memoryCacheKeyCountries,
            out ReadOnlyCollection<CountryResponseModel> countries) || (countries == null || !countries.Any()))
        {
          
            var _countries = await _countryRepository.GetAllAsync(c => 1 == 1);
            

            if (_countries != null && _countries.Any())
            {
                countries = _mapper.Map<ReadOnlyCollection<CountryResponseModel>>(_countries);
                _memoryCache.Set(_memoryCacheKeyCountries, countries.OrderBy(c => c.CountryName), new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.Date.AddDays(1).AddTicks(-1)
                });
            }
        }

        return countries;
    }

    public async Task<CreateCountryResponseModel> CreateAsync(CreateCountryModel createCountryModel)
    {
        try
        {
            var country = _mapper.Map<Country>(createCountryModel);
            var addedCountry = await _countryRepository.AddAsync(country);

            return new CreateCountryResponseModel
            {
                Id = addedCountry.Id
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<BaseResponseModel> DeleteAsync(Guid id)
    {
        var country = await _countryRepository.GetFirstAsync(tl => tl.Id == id);

        return new BaseResponseModel
        {
            Id = (await _countryRepository.DeleteAsync(country)).Id
        };
    }

    public async Task<UpdateCountryResponseModel> UpdateAsync(Guid id, UpdateCountryModel updateCountryModel)
    {
        var country = await _countryRepository.GetFirstAsync(ti => ti.Id == id);

        _mapper.Map(updateCountryModel, country);

        return new UpdateCountryResponseModel
        {
            Id = (await _countryRepository.UpdateAsync(country)).Id
        };
    }

    public async Task<AdminLevelResponseModel> GetAllAdminLevels(string countryName)
    {
        AdminLevelResponseModel adminLevel =new AdminLevelResponseModel();
            var _countries = await _countryRepository.GetAllAsync(c => c.CountryName.ToLower().Equals(countryName.ToLower()));
        if (_countries != null)
        {
            var country= _countries.FirstOrDefault();
            adminLevel.Country = _mapper.Map<CountryResponseModel>(country);
            var _counties = await _adminLevel1Repository.GetAllAsync(c => c.CountryId == country.Id);

            adminLevel.AdminLevel1 = _mapper.Map<ReadOnlyCollection<AdminLevel1ResponseModel>>(_counties);

            var allSubCounties = new List<AdminLevel2ResponseModel>(); 

            foreach (var county in _counties)
            {
                
                var _subCounties = await _adminLevel2Repository.GetAllAsync(sc => sc.CountyId == county.Id);
                var subCounties = _mapper.Map<ReadOnlyCollection<AdminLevel2ResponseModel>>(_subCounties);
                allSubCounties.AddRange(subCounties);
               
            }
            adminLevel.AdminLevel2 = _mapper.Map<ReadOnlyCollection<AdminLevel2ResponseModel>>(allSubCounties);

            var allWards = new List<AdminLevel3ResponseModel>();
            foreach (var subCounty in allSubCounties)
            {
                var _ward = await _adminLevel3Repository.GetAllAsync(sc => sc.SubCountyId == subCounty.Id);

                var wards = _mapper.Map<ReadOnlyCollection<AdminLevel3ResponseModel>>(_ward);
                allWards.AddRange(wards);
            }
            adminLevel.AdminLevel3 = _mapper.Map<ReadOnlyCollection<AdminLevel3ResponseModel>>(allWards);
           
        }
        return adminLevel;
    }
    #endregion
}
