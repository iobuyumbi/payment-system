using AutoMapper;
using LinqToDB.Common.Internal.Cache;
using Microsoft.Extensions.Caching.Memory;
using Solidaridad.Application.Models.Ward;
using Solidaridad.DataAccess.Repositories;
using System.Collections.ObjectModel;

namespace Solidaridad.Application.Services.Impl;

public class WardService : IWardService
{

    #region DI
    private readonly IMapper _mapper;
    private readonly IWardRepository _wardRepository;
    private const string _memoryCacheKeyCountries = "DB-Wards";
    private readonly IMemoryCache _memoryCache;

    public WardService(IMapper mapper,
          IMemoryCache memoryCache,
          IWardRepository wardRepository)
    {
        _mapper = mapper;
        _wardRepository = wardRepository;
        _memoryCache = memoryCache;
    }
    #endregion

    #region Methods
    public async Task<ReadOnlyCollection<WardResponseModel>> GetAllAsync()
    {
        if (!_memoryCache.TryGetValue<ReadOnlyCollection<WardResponseModel>>(
            _memoryCacheKeyCountries,
            out ReadOnlyCollection<WardResponseModel> wards) || (wards == null || !wards.Any()))
        {
            var _wards = await _wardRepository.GetAllAsync(c => 1 == 1);
            if (_wards != null && _wards.Any())
            {
                wards = _mapper.Map<ReadOnlyCollection<WardResponseModel>>(_wards);
                _memoryCache.Set(_memoryCacheKeyCountries, wards, new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.Date.AddDays(1).AddTicks(-1)
                });
            }
        }

        return wards;
    }
    #endregion
}
