using AutoMapper;
using FluentValidation;
using LinqToDB.Common.Internal.Cache;
using Microsoft.Extensions.Caching.Memory;
using NPOI.SS.Formula.Functions;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.Validators.AdminLevel1;
using Solidaridad.Application.Models.Validators.AdminLevel2;
using Solidaridad.Application.Models.Validators.AdminLevel3;
using Solidaridad.Application.Models.Ward;
using Solidaridad.Core.Entities;
using Solidaridad.Core.Entities.Base;
using Solidaridad.DataAccess.Repositories;
using Solidaridad.DataAccess.Repositories.Impl;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;

namespace Solidaridad.Application.Services.Impl;

public class AdminLevel3Service : IAdminLevel3Service
{

    #region DI
    private readonly IMapper _mapper;
    private readonly IAdminLevel1Repository _countyRepository;
    private readonly IAdminLevel3Repository _wardRepository;
    private readonly IAdminLevel2Repository _subCountyRepository;
    private const string _memoryCacheKeyCountries = "DB-Wards";
    private readonly IMemoryCache _memoryCache;
    private readonly CreateAdminLevel3Validator _createAdminLevel3Validator;
    private readonly UpdateAdminLevel3Validator _updateAdminLevel3Validator;

    public AdminLevel3Service(IMapper mapper,
          IMemoryCache memoryCache,
          IAdminLevel3Repository wardRepository,
          IAdminLevel2Repository subCountyRepository,
          IAdminLevel1Repository countyRepository)
    {
        _mapper = mapper;
        _wardRepository = wardRepository;
        _memoryCache = memoryCache;
        _createAdminLevel3Validator = new CreateAdminLevel3Validator();
        _updateAdminLevel3Validator = new UpdateAdminLevel3Validator();
        _subCountyRepository = subCountyRepository;
        _countyRepository = countyRepository;
    }
    #endregion

    #region Methods
    public async Task<ReadOnlyCollection<AdminLevel3ResponseModel>> GetAllAsync(AdminLevel3SearchParams searchParams)
    {
        var _wards = new List<AdminLevel3>();
        var counties = await _countyRepository.GetAllAsync(c => searchParams.CountryId == c.CountryId);

        var countyIds = counties.Select(c => c.Id).ToList();
        var subCounties = await _subCountyRepository.GetAllAsync(c => countyIds.Contains((Guid)c.CountyId));

        if (searchParams.SubCountyId != null)
        {
            _wards = await _wardRepository.GetAllAsync(c =>  (c.SubCountyId == searchParams.SubCountyId.Value)
            && (string.IsNullOrEmpty(searchParams.Filter) ||
            c.WardName.Contains(searchParams.Filter) ||
            c.WardCode.Contains(searchParams.Filter)));
        }
        else
        {
            var subCountyIds = subCounties.Select(c => c.Id).ToList();
            _wards = await _wardRepository.GetAllAsync(c => subCountyIds.Contains((Guid)c.SubCountyId) && (string.IsNullOrEmpty(searchParams.Filter) ||
            c.WardName.Contains(searchParams.Filter) ||
            c.WardCode.Contains(searchParams.Filter)));
        }
        var wards = _mapper.Map<ReadOnlyCollection<AdminLevel3ResponseModel>>(_wards);

        foreach (var item in wards)
        {
            item.SubCountyName = subCounties.FirstOrDefault(c => c.Id == item.SubCountyId).SubCountyName;
        }


        return wards;
    }

    public async Task<CreateAdminLevel3ResponseModel> CreateAsync(CreateAdminLevel3Model createWardModel)
    {
        try
        {
            var validationResult = _createAdminLevel3Validator.Validate(createWardModel);
            if (!validationResult.IsValid)
            {

                throw new ValidationException("Validation failed", validationResult.Errors);
            }
            var ward = _mapper.Map<AdminLevel3>(createWardModel);
            var addedWard = await _wardRepository.AddAsync(ward);

            return new CreateAdminLevel3ResponseModel
            {
                Id = addedWard.Id
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
        var ward = await _wardRepository.GetFirstAsync(tl => tl.Id == id);

        return new BaseResponseModel
        {
            Id = (await _wardRepository.DeleteAsync(ward)).Id
        };
    }

    public async Task<UpdateAdminLevel3ResponseModel> UpdateAsync(Guid id, UpdateAdminLevel3Model updateWardModel)
    {
        try
        {
            var validationResult = _updateAdminLevel3Validator.Validate(updateWardModel);
            if (!validationResult.IsValid)
            {

                throw new ValidationException("Validation failed", validationResult.Errors);
            }
            var ward = await _wardRepository.GetFirstAsync(ti => ti.Id == id);

        _mapper.Map(updateWardModel, ward);

        return new UpdateAdminLevel3ResponseModel
        {
            Id = (await _wardRepository.UpdateAsync(ward)).Id
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
    #endregion
}
