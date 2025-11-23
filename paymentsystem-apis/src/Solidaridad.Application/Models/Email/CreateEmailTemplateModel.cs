namespace Solidaridad.Application.Models.Email;

public class CreateEmailTemplateModel
{
    public string Name { get; set; }

    public string Subject { get; set; }

    public string Body { get; set; }

    public List<CreateEmailTemplateVariableModel> Variables { get; set; }
}

public class CreateEmailTemplateResponseModel : BaseResponseModel { }
