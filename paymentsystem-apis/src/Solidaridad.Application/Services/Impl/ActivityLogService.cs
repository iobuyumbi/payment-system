using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Solidaridad.Application.Exceptions;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.ActivityLog;
using Solidaridad.Core.Entities;
using Solidaridad.DataAccess.Identity;
using Solidaridad.DataAccess.Repositories;
using Solidaridad.Shared.Services;
using System.Globalization;

namespace Solidaridad.Application.Services.Impl;

public class ActivityLogService : IActivityLogService
{
    #region DI
    private readonly IClaimService _claimService;
    private readonly IMapper _mapper;
    private readonly IActivityLogRepository _activityLogRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public ActivityLogService(IActivityLogRepository activityLogRepository, 
        UserManager<ApplicationUser> userManager,
        IMapper mapper, IClaimService claimService)
    {
        _activityLogRepository = activityLogRepository;
        _userManager = userManager;
        _mapper = mapper;
        _claimService = claimService;
    }
    #endregion

    #region Methods
    public async Task<CreateActivityLogResponseModel> CreateAsync(CreateActivityLogModel createActivityLogModel, CancellationToken cancellationToken = default)
    {
        try
        {
            var activityLog = _mapper.Map<ActivityLog>(createActivityLogModel);

            activityLog.CreatedOn = DateTime.UtcNow;
            activityLog.CreatedBy =_claimService.GetUserEmail();

            var addedActivityLog = await _activityLogRepository.AddAsync(activityLog);

            return new CreateActivityLogResponseModel
            {
                Id = addedActivityLog.Id
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<BaseResponseModel> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var activityLog = await _activityLogRepository.GetFirstAsync(tl => tl.Id == id);

        return new BaseResponseModel
        {
            Id = (await _activityLogRepository.DeleteAsync(activityLog)).Id
        };
    }

    public async Task<IEnumerable<ActivityLogResponseModel>> GetAllAsync(string keyword, CancellationToken cancellationToken = default)
    {
        var activityLogs = await _activityLogRepository.GetAllAsync(tl => string.IsNullOrEmpty(tl.Link) == false);

        if (!string.IsNullOrEmpty(keyword))
        {
            activityLogs = activityLogs.Where(e =>
                         CultureInfo.CurrentCulture.CompareInfo.IndexOf(e.Title, keyword, CompareOptions.IgnoreCase) >= 0
                    ).ToList();
        }
        
        var list = _mapper.Map<IEnumerable<ActivityLogResponseModel>>(activityLogs);
        // var users = await _userManager.Users.ToListAsync();
        //foreach (var item in list)
        //{
        //    item.CreatedBy = users.FirstOrDefault(c => c.Id == item.CreatedBy).Email;
        //}

        return list.OrderByDescending(c=>c.CreatedOn);
    }

    public async Task<UpdateActivityLogResponseModel> UpdateAsync(Guid id, UpdateActivityLogModel updateActivityLogModel, CancellationToken cancellationToken = default)
    {
        var activityLog = await _activityLogRepository.GetFirstAsync(tl => tl.Id == id);

        return new UpdateActivityLogResponseModel
        {
            Id = (await _activityLogRepository.UpdateAsync(activityLog)).Id
        };
    }
    #endregion
}
