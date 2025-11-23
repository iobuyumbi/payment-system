using AutoMapper;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.Location;
using Solidaridad.Core.Entities.Base;
using Solidaridad.Core.Entities.Locations;
using Solidaridad.DataAccess.Repositories;
using Solidaridad.Shared.Services;

namespace Solidaridad.Application.Services.Impl;

public class LocationService : ILocationService
{
    #region DI
    private IMapper _mapper;
    private ILocationRepository _locationRepository;
    private readonly IClaimService _claimService;

    public LocationService(ILocationRepository locationRepository,
        IClaimService claimService,
        IMapper mapper)
    {
        _locationRepository = locationRepository;
        _claimService = claimService;
        _mapper = mapper;
    }

    #endregion

    #region Methods
    public async Task<CreateLocationResponseModel> CreateAsync(CreateLocationModel createLoanItemModel)
    {
        try
        {
            var location = _mapper.Map<Location>(createLoanItemModel);

            location.CreatedBy = Guid.Parse(_claimService.GetUserId());
            location.CreatedOn = DateTime.UtcNow;

            var addedItem = await _locationRepository.AddAsync(location);

            return new CreateLocationResponseModel
            {
                Id = addedItem.Id
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<BaseResponseModel> DeleteAsync(Guid id)
    {

        var location = await _locationRepository.GetFirstAsync(tl => tl.Id == id);
        location.IsDeleted = true;
        return new BaseResponseModel
        {
            Id = (await _locationRepository.UpdateAsync(location)).Id
        };
    }

    public async Task<IEnumerable<LocationResponseModel>> GetAllAsync(SearchParams searchParams)
    {
        var _locations = await _locationRepository.GetAllAsync(c =>
            string.IsNullOrEmpty(searchParams.Filter) ||
            c.Name.Contains(searchParams.Filter) && c.IsDeleted == false
        );

        var location = _mapper.Map<IEnumerable<LocationResponseModel>>(_locations);
        return location;

    }

    public async Task<UpdateLocationResponseModel> UpdateAsync(Guid id, UpdateLocationModel updateLoanItemModel)
    {
        var location = await _locationRepository.GetFirstAsync(ti => ti.Id == id);

        location.UpdatedBy = Guid.Parse(_claimService.GetUserId());
        location.UpdatedOn = DateTime.UtcNow;

        _mapper.Map(updateLoanItemModel, location);

        return new UpdateLocationResponseModel
        {
            Id = (await _locationRepository.UpdateAsync(location)).Id
        };
    }

    public async Task<LocationResponseModel> GetByIdAsync(Guid id, Guid countryId)
    {
        var _locations = await _locationRepository.GetFirstAsync(c => c.Id == id && c.CountryId == countryId);

        var location = _mapper.Map<LocationResponseModel>(_locations);
        return location;
    }
    #endregion
}
