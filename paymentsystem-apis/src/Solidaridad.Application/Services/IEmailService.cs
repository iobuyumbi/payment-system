using Solidaridad.Application.Common.Email;

namespace Solidaridad.Application.Services;

public interface IEmailService
{
    Task SendEmailAsync(EmailMessage emailMessage);
}
