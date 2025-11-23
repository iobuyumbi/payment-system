using AutoMapper;
using Solidaridad.Application.Models.Project;
using Solidaridad.Core.Entities;

namespace Solidaridad.Application.MappingProfiles;

public class ProjectProfile : Profile
{
    public ProjectProfile()
    {
        CreateMap<Project, ProjectResponseModel>();

        CreateMap<ProjectResponseModel, Project>();
        
        CreateMap<CreateProjectModel, Project>();
        
        CreateMap<UpdateProjectModel, Project>();
    }
}
