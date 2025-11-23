using AutoMapper;
using LinqToDB.Common.Internal.Cache;
using Microsoft.Extensions.Caching.Memory;
using Solidaridad.Application.Models.County;
using Solidaridad.DataAccess.Repositories;
using System.Collections.ObjectModel;

namespace Solidaridad.Application.Services.Impl;

public class CountyService : ICountyService
{

    #region DI
    private readonly IMapper _mapper;
    private readonly ICountyRepository _countyRepository;
    private const string _memoryCacheKeyCountries = "DB-Counties";
    private readonly IMemoryCache _memoryCache;

    public CountyService(IMapper mapper,
          IMemoryCache memoryCache,
          ICountyRepository countyRepository)
    {
        _mapper = mapper;
        _countyRepository = countyRepository;
        _memoryCache = memoryCache;
    }
    #endregion

    #region Methods
    public async Task<ReadOnlyCollection<CountyResponseModel>> GetAllAsync()
    {
        if (!_memoryCache.TryGetValue<ReadOnlyCollection<CountyResponseModel>>(
            _memoryCacheKeyCountries,
            out ReadOnlyCollection<CountyResponseModel> counties) || (counties == null || !counties.Any()))
        {
            var _counties = await _countyRepository.GetAllAsync(c => 1 == 1);
            if (_counties != null && _counties.Any())
            {
                counties = _mapper.Map<ReadOnlyCollection<CountyResponseModel>>(_counties);
                _memoryCache.Set(_memoryCacheKeyCountries, counties, new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.Date.AddDays(1).AddTicks(-1)
                });
            }
        }

        return counties;
    }
    #endregion
}
