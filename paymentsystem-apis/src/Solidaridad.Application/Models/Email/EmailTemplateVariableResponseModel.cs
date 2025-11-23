namespace Solidaridad.Application.Models.Email;

public class EmailTemplateVariableResponseModel : BaseResponseModel
{
    public string Name { get; set; }

    public string DefaultValue { get; set; }
}
