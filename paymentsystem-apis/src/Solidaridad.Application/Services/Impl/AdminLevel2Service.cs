using AutoMapper;
using FluentValidation;
using NPOI.SS.Formula.Functions;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.Country;
using Solidaridad.Application.Models.County;
using Solidaridad.Application.Models.SubCounty;
using Solidaridad.Application.Models.Validators.AdminLevel1;
using Solidaridad.Application.Models.Validators.AdminLevel2;
using Solidaridad.Application.Models.Validators.AdminLevel3;
using Solidaridad.Core.Entities;
using Solidaridad.Core.Entities.Base;
using Solidaridad.DataAccess.Repositories;
using Solidaridad.DataAccess.Repositories.Impl;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;

namespace Solidaridad.Application.Services.Impl;

public class AdminLevel2Service : IAdminLevel2Service
{
    #region DI
    private readonly IMapper _mapper;
    private readonly IAdminLevel1Repository _countyRepository;
    private readonly IAdminLevel2Repository _subCountyRepository;
    private const string _memoryCacheKeyCountries = "DB-Counties";
    private readonly CreateAdminLevel2Validator _createAdminLevel2Validator;
    private readonly UpdateAdminLevel2Validator _updateAdminLevel2Validator;

    public AdminLevel2Service(IMapper mapper,

          IAdminLevel2Repository subCountyRepository,
          IAdminLevel1Repository countyRepository)
    {
        _mapper = mapper;
        _subCountyRepository = subCountyRepository;
        _createAdminLevel2Validator = new CreateAdminLevel2Validator();
        _updateAdminLevel2Validator = new UpdateAdminLevel2Validator();
        _countyRepository = countyRepository;
    }
    #endregion
    public async Task<CreateAdminLevel2ResponseModel> CreateAsync(CreateAdminLevel2Model createSubCountyModel)
    {
        try
        {
            var validationResult = _createAdminLevel2Validator.Validate(createSubCountyModel);
            if (!validationResult.IsValid)
            {

                throw new ValidationException("Validation failed", validationResult.Errors);
            }
            var subCounty = _mapper.Map<AdminLevel2>(createSubCountyModel);
            var addedsubCounty = await _subCountyRepository.AddAsync(subCounty);

            return new CreateAdminLevel2ResponseModel
            {
                Id = addedsubCounty.Id
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
        var county = await _subCountyRepository.GetFirstAsync(tl => tl.Id == id);

        return new BaseResponseModel
        {
            Id = (await _subCountyRepository.DeleteAsync(county)).Id
        };
    }

    public async Task<ReadOnlyCollection<AdminLevel2ResponseModel>> GetAllAsync(AdminLevel2SearchParams searchParams)
    {

        var _subCounties = new List<AdminLevel2>();
        var counties = await _countyRepository.GetAllAsync(c => searchParams.CountryId == c.CountryId);
        if (searchParams.CountyId != null)
        {
            _subCounties = await _subCountyRepository.GetAllAsync(c => (c.CountyId == searchParams.CountyId.Value)
            && (string.IsNullOrEmpty(searchParams.Filter) ||
            c.SubCountyName.Contains(searchParams.Filter) ||
            c.SubCountyCode.Contains(searchParams.Filter)));
        }
        else
        {
            var countyIds = counties.Select(c => c.Id).ToList();
            _subCounties = await _subCountyRepository.GetAllAsync(c => countyIds.Contains((Guid)c.CountyId) && (string.IsNullOrEmpty(searchParams.Filter) ||
            c.SubCountyName.Contains(searchParams.Filter) ||
            c.SubCountyCode.Contains(searchParams.Filter))   );
        }

        var subCounties = _mapper.Map<ReadOnlyCollection<AdminLevel2ResponseModel>>(_subCounties);

        foreach (var item in subCounties)
        {
            item.CountyName = counties.FirstOrDefault(c => c.Id == item.CountyId).CountyName;
        }

        return subCounties;
    }

    public async Task<UpdateAdminLevel2ResponseModel> UpdateAsync(Guid id, UpdateAdminLevel2Model updateSubCountyModel)
    {
        try
        {
            var validationResult = _updateAdminLevel2Validator.Validate(updateSubCountyModel);
            if (!validationResult.IsValid)
            {

                throw new ValidationException("Validation failed", validationResult.Errors);
            }
            var _subCounty = await _subCountyRepository.GetAllAsync(ti => ti.Id == id);
        var subCounty = _subCounty.FirstOrDefault();
        _mapper.Map(updateSubCountyModel, subCounty);

        return new UpdateAdminLevel2ResponseModel
        {
            Id = (await _subCountyRepository.UpdateAsync(subCounty)).Id
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
}
