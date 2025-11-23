using Solidaridad.Application.Models.AuditLog;

namespace Solidaridad.Application.Services;

public interface IAuditLogService
{
    Task<List<AuditLogResponse>> GetAuditLog<TEntity>(string entity, string entityId);
    Task<List<AuditLogResponse>> GetTopAuditLogs<TEntity>(string entity, int limit);
    Task LogChanges<T>(string oldValuesJson, string newValuesJson, Guid entityId, string entityType, Guid changedBy, Guid orgId);
}
