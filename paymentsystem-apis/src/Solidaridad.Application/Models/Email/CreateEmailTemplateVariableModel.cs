namespace Solidaridad.Application.Models.Email;

public class CreateEmailTemplateVariableModel
{
    public string Name { get; set; }

    public string DefaultValue { get; set; }
}

public class CreateEmailTemplateVariableResponseModel : BaseResponseModel { }
