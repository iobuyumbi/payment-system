using Solidaridad.Application.Models;
using Solidaridad.Application.Models.ActivityLog;

namespace Solidaridad.Application.Services;

public interface IActivityLogService
{
    Task<CreateActivityLogResponseModel> CreateAsync(CreateActivityLogModel createActivityLogModel,
        CancellationToken cancellationToken = default);

    Task<BaseResponseModel> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IEnumerable<ActivityLogResponseModel>>
        GetAllAsync(string keyword,CancellationToken cancellationToken = default);

    Task<UpdateActivityLogResponseModel> UpdateAsync(Guid id, UpdateActivityLogModel updateActivityLogModel,
        CancellationToken cancellationToken = default);
}
