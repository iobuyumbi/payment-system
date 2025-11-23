using Microsoft.Extensions.Configuration;
using Npgsql;
using Solidaridad.Application.Models.ActivityLog;
using Solidaridad.DataAccess;

namespace Solidaridad.Application.Services.Impl;

public class BaseService
{
    private readonly string _connectionString;
    private readonly IActivityLogService _activityLogService;

    public BaseService(IActivityLogService activityLogService)
    {
        _activityLogService = activityLogService;
    }

    public BaseService(IConfiguration configuration)
    {
        _connectionString = configuration.GetSection("ConnectionStrings").Get<DatabaseConfiguration>().DefaultConnection;
    }

    protected NpgsqlConnection GetConnection()
    {
         return new NpgsqlConnection(_connectionString);
    }

    public async Task SaveActivityLog(CreateActivityLogModel createActivityLogModel)
    {
        await _activityLogService.CreateAsync(createActivityLogModel);
    }
}
