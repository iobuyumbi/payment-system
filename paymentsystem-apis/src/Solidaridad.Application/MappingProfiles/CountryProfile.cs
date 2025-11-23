using AutoMapper;
using Solidaridad.Application.Models.Country;
using Solidaridad.Application.Models.Farmer;
using Solidaridad.Core.Entities;

namespace Solidaridad.Application.MappingProfiles;

public class CountryProfile : Profile
{
    public CountryProfile() {
        CreateMap<Country, CountryResponseModel>();
        
        CreateMap<CreateCountryModel, Country>();

        CreateMap<CountryResponseModel, Country>();
       
        CreateMap<CreateModuleModel, Country>();

        CreateMap<UpdateCountryModel, Country>();
    }
}
