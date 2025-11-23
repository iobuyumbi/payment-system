using Solidaridad.Application.Models;
using Solidaridad.Application.Models.Email;
using Solidaridad.Core.Entities.Email;

namespace Solidaridad.Application.Services;

public interface IEmailTemplateService
{
    Task<CreateEmailTemplateResponseModel> CreateAsync(CreateEmailTemplateModel createEmailTemplate, CancellationToken cancellationToken = default);
    
    Task<BaseResponseModel> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task<IEnumerable<EmailTemplateResponseModel>> GetAllAsync(string keyword, CancellationToken cancellationToken = default);
    
    Task<EmailTemplateResponseModel> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task<EmailTemplateResponseModel> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    
    string RenderTemplate(EmailTemplate template, Dictionary<string, string> variables);
    
    Task<UpdateEmailTemplateResponseModel> UpdateAsync(Guid id, UpdateEmailTemplateModel updateEmailTemplate, CancellationToken cancellationToken = default);
}
