using AutoMapper;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.LoanItem;
using Solidaridad.Application.Models.LocationProfiles;
using Solidaridad.Core.Entities.Base;
using Solidaridad.Core.Entities.Locations;
using Solidaridad.DataAccess.Repositories;
using Solidaridad.Shared.Services;

namespace Solidaridad.Application.Services.Impl;

public class LocationProfileService : ILocationProfileService
{
    #region DI
    private IMapper _mapper;
    private ILocationProfileRepository _locationProfileRepository;
    private readonly IClaimService _claimService;
    private readonly IAttachmentUploadRepository _attachmentRepository;

    public LocationProfileService(ILocationProfileRepository locationProfileRepository, IAttachmentUploadRepository attachmentRepository,
        IClaimService claimService,
        IMapper mapper)
    {
        _locationProfileRepository = locationProfileRepository;
        _claimService = claimService;
        _mapper = mapper;
        _attachmentRepository = attachmentRepository;
    }

    public async Task<CreateLocationProfileResponseModel> CreateAsync(CreateLocationProfileModel createLoanItemModel)
    {
        try
        {
            var locationProfile = _mapper.Map<LocationProfile>(createLoanItemModel);

            // Fixing CS0443: Syntax error; value expected
            // The issue is with the incorrect usage of `createLoanItemModel.AttachmentIds[]`.
            // Instead, we should access a specific index or iterate over the array.
            if (createLoanItemModel.AttachmentIds != null && createLoanItemModel.AttachmentIds.Length > 0)
            {
                var firstAttachmentId = new Guid(createLoanItemModel.AttachmentIds[0]);
                locationProfile.LogoUrl = (await _attachmentRepository.GetFirstAsync(c => c.Id == firstAttachmentId))?.ImagePath;
            }

            locationProfile.CreatedBy = Guid.Parse(_claimService.GetUserId());
            locationProfile.CreatedOn = DateTime.UtcNow;

            var addedItem = await _locationProfileRepository.AddAsync(locationProfile);

            return new CreateLocationProfileResponseModel
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

        var locationProfile = await _locationProfileRepository.GetFirstAsync(tl => tl.Id == id);
        locationProfile.IsDeleted = true;
        return new BaseResponseModel
        {
            Id = (await _locationProfileRepository.UpdateAsync(locationProfile)).Id
        };
    }

    public async Task<IEnumerable<LocationProfileResponseModel>> GetAllAsync(SearchParams searchParams)
    {
        var _locationProfiles = await _locationProfileRepository.GetAllAsync(c =>
            (string.IsNullOrEmpty(searchParams.Filter) || c.City.Contains(searchParams.Filter))
                && c.IsDeleted == false
                && c.CountryId == searchParams.CountryId
        );

        var locationProfile = _mapper.Map<IEnumerable<LocationProfileResponseModel>>(_locationProfiles);
        return locationProfile;

    }

    public async Task<UpdateLocationProfileResponseModel> UpdateAsync(Guid id, UpdateLocationProfileModel updateLoanItemModel)
    {
        var locationProfile = await _locationProfileRepository.GetFirstAsync(ti => ti.Id == id);

        locationProfile.UpdatedBy = Guid.Parse(_claimService.GetUserId());
        locationProfile.UpdatedOn = DateTime.UtcNow;
        if (updateLoanItemModel.AttachmentIds != null && updateLoanItemModel.AttachmentIds.Length > 0)
        {
            var firstAttachmentId = new Guid(updateLoanItemModel.AttachmentIds[0]);
            updateLoanItemModel.LogoUrl = (await _attachmentRepository.GetFirstAsync(c => c.Id == firstAttachmentId))?.ImagePath;
        }
        _mapper.Map(updateLoanItemModel, locationProfile);

        return new UpdateLocationProfileResponseModel
        {
            Id = (await _locationProfileRepository.UpdateAsync(locationProfile)).Id
        };
    }

    public async Task<LocationProfileResponseModel> GetByIdAsync(Guid id)
    {
        var _locationProfiles = await _locationProfileRepository.GetFirstAsync(c => c.Id == id);

        var locationProfile = _mapper.Map<LocationProfileResponseModel>(_locationProfiles);
        return locationProfile;
    }
    #endregion
}
