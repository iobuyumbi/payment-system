using Newtonsoft.Json;
using Solidaridad.Application.Helpers;
using Solidaridad.Application.Models.AuditLog;
using Solidaridad.Core.Entities;
using Solidaridad.DataAccess.Repositories;
using Solidaridad.Shared.Services;

namespace Solidaridad.Application.Services.Impl;

public class AuditLogService : IAuditLogService
{
    #region DI
    private readonly IAuditLogRepository _auditLogRepository;
    private readonly IClaimService _claimService;

    public AuditLogService(IAuditLogRepository auditLogRepository,
        IClaimService claimService)
    {
        _auditLogRepository = auditLogRepository;
        _claimService = claimService;
    }
    #endregion

    public async Task LogChanges<T>(string oldValuesJson, string newValuesJson, Guid entityId, string entityType, Guid changedBy, Guid orgId)
    {
        // Deserialize JSON into single objects instead of lists
        var oldValues = JsonConvert.DeserializeObject<T>(oldValuesJson);
        var newValues = JsonConvert.DeserializeObject<T>(newValuesJson);

        // Ensure both objects are not null
        if (oldValues == null || newValues == null)
            return;

        // Get changed fields
        var changes = AuditHelper.GetChangedFields<T>(oldValuesJson, newValuesJson);

        foreach (var change in changes)
        {
            var logEntry = new AuditLog
            {
                EntityType = entityType,
                EntityId = entityId.ToString(),
                ChangedBy = changedBy.ToString(),
                ChangedOn = DateTime.UtcNow,
                ChangeType = "Updated",
                OldValue = (string)change.OldValue,
                NewValue = (string)change.NewValue,
            };

            await _auditLogRepository.AddAsync(logEntry);
        }
    }

    public async Task<List<AuditLogResponse>> GetAuditLog<TEntity>(string entity, string entityId)
    {
        try
        {
            var auditLogs = await _auditLogRepository.GetAllAsync(c => c.EntityId == entityId && c.EntityType.ToLower() == entity.ToLower());

            return auditLogs
                .OrderByDescending(log => log.ChangedOn)
                .Select(log => new AuditLogResponse
                {
                    ChangedBy = Convert.ToString(log.ChangedBy),
                    ChangedOn = log.ChangedOn,
                    ChangeType = log.ChangeType,
                    Changes = AuditHelper.GetChangedFields<TEntity>(log.OldValue, log.NewValue) // Generic Type
                })
                .ToList();
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    public async Task<List<AuditLogResponse>> GetTopAuditLogs<TEntity>(string entity, int limit)
    {
        try
        {
            var auditLogs = await _auditLogRepository
                .GetAllAsync(c => c.EntityType.ToLower() == entity.ToLower());

            return auditLogs
                .OrderByDescending(log => log.ChangedOn)
                .Take(limit)
                .Select(log => new AuditLogResponse
                {
                    ChangedBy = Convert.ToString(log.ChangedBy),
                    ChangedOn = log.ChangedOn,
                    ChangeType = log.ChangeType,
                    Changes = AuditHelper.GetChangedFields<TEntity>(log.OldValue, log.NewValue)
                })
                .ToList();
        }
        catch (Exception)
        {
            throw;
        }
    }
}
