using Solidaridad.Core.Entities;

namespace Solidaridad.Application.Services;

public interface IApiRequestLogger
{
    Task LogAsync(ApiRequestLog requestLog);
}
