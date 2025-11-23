using AutoMapper;
using LinqToDB.Common.Internal.Cache;
using Microsoft.Extensions.Caching.Memory;
using Solidaridad.Application.Models.Constituency;
using Solidaridad.DataAccess.Repositories;
using System.Collections.ObjectModel;

namespace Solidaridad.Application.Services.Impl;

public class ConstituencyService : IConstituencyService
{

    #region DI
    private readonly IMapper _mapper;
    private readonly IConstituencyRepository _constituencyRepository;
    private const string _memoryCacheKeyCountries = "DB-Constituencies";
    private readonly IMemoryCache _memoryCache;

    public ConstituencyService(IMapper mapper,
          IMemoryCache memoryCache,
          IConstituencyRepository constituencyRepository)
    {
        _mapper = mapper;
        _constituencyRepository = constituencyRepository;
        _memoryCache = memoryCache;
    }
    #endregion

    #region Methods
    public async Task<ReadOnlyCollection<ConstituencyResponseModel>> GetAllAsync()
    {
        if (!_memoryCache.TryGetValue<ReadOnlyCollection<ConstituencyResponseModel>>(
            _memoryCacheKeyCountries,
            out ReadOnlyCollection<ConstituencyResponseModel> constituencies) || (constituencies == null || !constituencies.Any()))
        {
            var _constituencies = await _constituencyRepository.GetAllAsync(c => 1 == 1);
            if (_constituencies != null && _constituencies.Any())
            {
                constituencies = _mapper.Map<ReadOnlyCollection<ConstituencyResponseModel>>(_constituencies);
                _memoryCache.Set(_memoryCacheKeyCountries, constituencies, new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.Date.AddDays(1).AddTicks(-1)
                });
            }
        }

        return constituencies;
    }
    #endregion
}
