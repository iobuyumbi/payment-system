using System.Threading.Tasks;
using Solidaridad.Application.Common.Email;
using Solidaridad.Application.Services;

namespace Solidaridad.Api.IntegrationTests.Helpers.Services;

public class EmailTestService : IEmailService
{
    public async Task SendEmailAsync(EmailMessage emailMessage)
    {
        await Task.Delay(100);
    }
}
