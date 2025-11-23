using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Solidaridad.Application.Helpers;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.AuditLog;
using Solidaridad.Application.Services;
using Solidaridad.Application.Services.Impl;
using Solidaridad.Core.Entities;
using Solidaridad.Core.Entities.Loans;
using Solidaridad.Core.Entities.Payments;
using Solidaridad.Core.Enums;

namespace Solidaridad.API.Controllers;

[Authorize]
public class AuditLogController : ApiController
{
    #region DI

    private readonly IAuditLogService _auditLogService;

    public AuditLogController(IAuditLogService auditLogService)
    {
        _auditLogService = auditLogService;
    }

    #endregion


    [HttpGet("{exModule}/{componentId}")]
    public async Task<IActionResult> GetAuditLog(string exModule, string componentId)
    {
        object auditLog = exModule switch
        {
            AuditConstants.FARMER => await _auditLogService.GetAuditLog<Farmer>(exModule, componentId),
            AuditConstants.LOAN_APPLICATION => await _auditLogService.GetAuditLog<LoanApplication>(exModule, componentId),
            AuditConstants.PAYMENT_BATCH => await _auditLogService.GetAuditLog<PaymentBatch>(exModule, componentId),
            AuditConstants.LOAN_BATCH => await _auditLogService.GetAuditLog<LoanBatch>(exModule, componentId),
            AuditConstants.USER => await _auditLogService.GetAuditLog<User>(exModule, componentId),
            _ => null
        };

        if (auditLog == null)
            return BadRequest();

        return Ok(ApiResult<List<AuditLogResponse>>.Success((List<AuditLogResponse>)auditLog));
    }

    [HttpGet("top/{exModule}/{limit}")]
    public async Task<IActionResult> GetLatestAuditLogs(string exModule, int limit)
    {
        object auditLog = exModule switch
        {
            AuditConstants.FARMER => await _auditLogService.GetTopAuditLogs<Farmer>(exModule, limit),
            AuditConstants.LOAN_APPLICATION => await _auditLogService.GetTopAuditLogs<LoanApplication>(exModule, limit),
            AuditConstants.PAYMENT_BATCH => await _auditLogService.GetTopAuditLogs<PaymentBatch>(exModule, limit),
            AuditConstants.LOAN_BATCH => await _auditLogService.GetTopAuditLogs<LoanBatch>(exModule, limit),
            AuditConstants.USER => await _auditLogService.GetTopAuditLogs<User>(exModule, limit),
            AuditConstants.PAY_REQUEST_DEDUCTIBLE => await _auditLogService.GetTopAuditLogs<User>(exModule, limit),
            _ => null
        };

        if (auditLog == null)
            return BadRequest();

        return Ok(ApiResult<List<AuditLogResponse>>.Success((List<AuditLogResponse>)auditLog));
    }
}
