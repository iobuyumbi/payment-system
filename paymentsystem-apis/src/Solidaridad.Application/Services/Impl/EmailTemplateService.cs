using AutoMapper;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.Email;
using Solidaridad.Core.Entities.Email;
using Solidaridad.DataAccess.Repositories;
using Solidaridad.Shared.Services;
using System.Globalization;

namespace Solidaridad.Application.Services.Impl;

public class EmailTemplateService : IEmailTemplateService
{
    #region DI
    private readonly IClaimService _claimService;
    private readonly IMapper _mapper;
    private readonly IEmailTemplateRepository _emailTemplateRepository;
    private readonly IEmailTemplateVariableRepository _emailTemplateVariableRepository;

    public EmailTemplateService(IEmailTemplateRepository emailTemplateRepository,
        IEmailTemplateVariableRepository emailTemplateVariableRepository,
        IMapper mapper, IClaimService claimService)
    {
        _emailTemplateRepository = emailTemplateRepository;
        _emailTemplateVariableRepository = emailTemplateVariableRepository;
        _mapper = mapper;
        _claimService = claimService;
    }
    #endregion

    #region Methods
    public async Task<CreateEmailTemplateResponseModel> CreateAsync(CreateEmailTemplateModel createEmailTemplate, CancellationToken cancellationToken = default)
    {
        try
        {
            var emailTemplate = _mapper.Map<EmailTemplate>(createEmailTemplate);
            var addedEmp = await _emailTemplateRepository.AddAsync(emailTemplate);

            if (addedEmp != null)
            {
                var variables = _mapper.Map<IEnumerable<EmailTemplateVariable>>(createEmailTemplate.Variables);
                await _emailTemplateVariableRepository.AddRange(variables);
            }

            return new CreateEmailTemplateResponseModel
            {
                Id = addedEmp.Id
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<BaseResponseModel> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var contact = await _emailTemplateRepository.GetFirstAsync(tl => tl.Id == id);

        return new BaseResponseModel
        {
            Id = (await _emailTemplateRepository.DeleteAsync(contact)).Id
        };
    }

    public async Task<EmailTemplateResponseModel> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var emailTemplates = await _emailTemplateRepository.GetAllAsync(tl => tl.Id == id);

        var list = _mapper.Map<IEnumerable<EmailTemplateResponseModel>>(emailTemplates);
        list.FirstOrDefault().Variables = _mapper.Map<List<EmailTemplateVariableResponseModel>>(
                     await _emailTemplateVariableRepository.GetAllAsync(tl => tl.EmailTemplateId == id));

        return list.FirstOrDefault();
    }

    public async Task<EmailTemplateResponseModel> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var currentOrgId = Guid.Parse(_claimService.GetClaim("orgid"));

        var emailTemplates = await _emailTemplateRepository.GetAllAsync(tl => (tl.Name == name) ||
                                                                              (tl.IsSystemDefined == true && tl.Name == name));

        var list = _mapper.Map<IEnumerable<EmailTemplateResponseModel>>(emailTemplates);

        //list.FirstOrDefault().Variables = _mapper.Map<List<EmailTemplateVariableResponseModel>>(
        //             await _emailTemplateVariableRepository.GetAllAsync(tl => tl.EmailTemplateId == id));

        return list.FirstOrDefault();
    }

    public async Task<IEnumerable<EmailTemplateResponseModel>> GetAllAsync(string keyword, CancellationToken cancellationToken = default)
    {
        var emailTemplates = await _emailTemplateRepository.GetAllAsync(c => 1 == 1);

        if (!string.IsNullOrEmpty(keyword))
        {
            emailTemplates = emailTemplates.Where(e =>
                         CultureInfo.CurrentCulture.CompareInfo.IndexOf(e.Name, keyword, CompareOptions.IgnoreCase) >= 0).ToList();
        }

        var list = _mapper.Map<IEnumerable<EmailTemplateResponseModel>>(emailTemplates);

        return list;
    }

    public async Task<UpdateEmailTemplateResponseModel> UpdateAsync(Guid id, UpdateEmailTemplateModel updateEmailTemplate, CancellationToken cancellationToken = default)
    {
        var emailTemplate = await _emailTemplateRepository.GetFirstAsync(tl => tl.Id == id);

        emailTemplate.Name = updateEmailTemplate.Name;
        emailTemplate.Subject = updateEmailTemplate.Subject;
        emailTemplate.Body = updateEmailTemplate.Body;

        var updatedTemplate = await _emailTemplateRepository.UpdateAsync(emailTemplate);
        if (updatedTemplate != null)
        {
            // delete old variables
            var emailTemplateVariable = await _emailTemplateVariableRepository.GetAllAsync(tl => tl.EmailTemplateId == id);
            foreach (var items in emailTemplateVariable)
            {
                await _emailTemplateVariableRepository.DeleteAsync(items);
            }

            // add new range
            var variables = _mapper.Map<IEnumerable<EmailTemplateVariable>>(updateEmailTemplate.Variables);

            // set email templateId
            variables.ToList().ForEach(c => c.EmailTemplateId = id);

            await _emailTemplateVariableRepository.AddRange(variables);
        }

        return new UpdateEmailTemplateResponseModel
        {
            Id = updatedTemplate.Id
        };
    }

    public string RenderTemplate(EmailTemplate template, Dictionary<string, string> variables)
    {
        string body = template.Body;

        foreach (EmailTemplateVariable variable in template.Variables)
        {
            if (variables.TryGetValue(variable.Name, out string value))
            {
                body = body.Replace("{" + variable.Name + "}", value);
            }
            else
            {
                throw new KeyNotFoundException($"The variable '{variable.Name}' was not found in the provided dictionary.");
            }
        }

        return body;
    }

    #endregion
}
