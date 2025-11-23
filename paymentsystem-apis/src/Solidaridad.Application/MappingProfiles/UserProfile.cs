using AutoMapper;
using Solidaridad.Application.Models.User;
using Solidaridad.DataAccess.Identity;

namespace Solidaridad.Application.MappingProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<CreateUserModel, ApplicationUser>();

        CreateMap<ApplicationUser, LoginUserModel>();
        
        CreateMap<LoginUserModel, ApplicationUser>();

        CreateMap<ApplicationUser, UserResponseModel>();
        
        CreateMap<UserResponseModel, ApplicationUser>();
    }
}
