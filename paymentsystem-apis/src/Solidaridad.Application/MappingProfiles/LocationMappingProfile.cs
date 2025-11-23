using AutoMapper;
using Solidaridad.Application.Models.Location;
using Solidaridad.Application.Models.LocationProfiles;
using Solidaridad.Core.Entities.Locations;

namespace Solidaridad.Application.MappingProfiles;

public class LocationMappingProfile : Profile
{
    public LocationMappingProfile()
    {
        #region Location

        CreateMap<CreateLocationModel, Location>();

        CreateMap<Location, LocationResponseModel>();

        CreateMap<UpdateLocationModel, Location>();

        #endregion

        #region Location Profile

        CreateMap<CreateLocationProfileModel, LocationProfile>()
            .ForMember(dest => dest.AttachmentFileId, opt => opt.Ignore());

        // From LocationProfile to CreateLocationProfileModel
        CreateMap<LocationProfile, CreateLocationProfileModel>()
            .ForMember(dest => dest.AttachmentIds, opt => opt.Ignore());

        CreateMap<LocationProfile, LocationProfileResponseModel>();

        CreateMap<UpdateLocationProfileModel, LocationProfile>();

        #endregion
    }
}
