using Solidaridad.Application.Models;
using Solidaridad.Application.Models.Project;
using Solidaridad.Core.Entities.Base;

namespace Solidaridad.Application.Services;

public interface IProjectService
{
    Task<CreateProjectResponseModel> CreateAsync(CreateProjectModel project);

    Task<BaseResponseModel> DeleteAsync(Guid id);

    Task<IEnumerable<ProjectResponseModel>> GetAllAsync(ProjectSearchParams searchParams);

    Task<UpdateProjectResponseModel> UpdateAsync(Guid id, UpdateProjectModel projectModel);
    Task<IEnumerable<ProjectResponseModel>> GetAllByListIdAsync(Guid id, CancellationToken cancellationToken = default);
}
