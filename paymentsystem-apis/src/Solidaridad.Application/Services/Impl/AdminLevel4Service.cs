using AutoMapper;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.Village;
using Solidaridad.Application.Models.Ward;
using Solidaridad.Core.Entities;
using Solidaridad.DataAccess.Repositories;
using Solidaridad.DataAccess.Repositories.Impl;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solidaridad.Application.Services.Impl;

public class AdminLevel4Service : IAdminLevel4Service
{
    #region DI
    private readonly IMapper _mapper;
    private readonly IAdminLevel4Repository _villageRepository;
    private const string _memoryCacheKeyCountries = "DB-Wards";


    public AdminLevel4Service(IMapper mapper,

          IAdminLevel4Repository villageRepository)
    {
        _mapper = mapper;
        _villageRepository = villageRepository;

    }
    #endregion
    public async Task<CreateAdminLevel4ResponseModel> CreateAsync(CreateAdminLevel4Model createVillageModel)
    {
        try
        {
            var village = _mapper.Map<AdminLevel4>(createVillageModel);
            var addedVillage = await _villageRepository.AddAsync(village);

            return new CreateAdminLevel4ResponseModel
            {
                Id = addedVillage.Id
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<BaseResponseModel> DeleteAsync(Guid id)
    {
        var ward = await _villageRepository.GetFirstAsync(tl => tl.Id == id);

        return new BaseResponseModel
        {
            Id = (await _villageRepository.DeleteAsync(ward)).Id
        };
    }

    public async Task<ReadOnlyCollection<AdminLevel4ResponseModel>> GetAllAsync()
    {

        var _wards = await _villageRepository.GetAllAsync(c => 1 == 1);

        var wards = _mapper.Map<ReadOnlyCollection<AdminLevel4ResponseModel>>(_wards);

        return wards;
    }

    public async Task<UpdateAdminLevel4ResponseModel> UpdateAsync(Guid id, UpdateAdminLevel4Model updateVillageModel)
    {
        var village = await _villageRepository.GetFirstAsync(ti => ti.Id == id);

        _mapper.Map(updateVillageModel, village);

        return new UpdateAdminLevel4ResponseModel
        {
            Id = (await _villageRepository.UpdateAsync(village)).Id
        };
    }
}
