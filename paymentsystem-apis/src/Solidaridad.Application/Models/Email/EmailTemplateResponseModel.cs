namespace Solidaridad.Application.Models.Email;

public class EmailTemplateResponseModel: BaseResponseModel
{
     public string Name { get; set; }

    public string Subject { get; set; }

    public string Body { get; set; }

    public string CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public string UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public List<EmailTemplateVariableResponseModel> Variables { get; set; }
}
