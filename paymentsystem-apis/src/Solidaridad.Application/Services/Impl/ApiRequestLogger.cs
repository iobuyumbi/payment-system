using Microsoft.EntityFrameworkCore;
using Solidaridad.Core.Entities;

namespace Solidaridad.Application.Services.Impl;

public class ApiRequestLogger : IApiRequestLogger
{
    private readonly DbContext _dbContext;

    public ApiRequestLogger(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task LogAsync(ApiRequestLog requestLog)
    {
        //_dbContext.ApiRequestLogs.Add(requestLog); // Assuming `ApiRequestLogs` DbSet is defined
        //await _dbContext.SaveChangesAsync();
    }
}
