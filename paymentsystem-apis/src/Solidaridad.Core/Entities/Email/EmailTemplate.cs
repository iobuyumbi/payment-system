using Solidaridad.Core.Common;

namespace Solidaridad.Core.Entities.Email;

public class EmailTemplate : BaseEntity, IAuditedEntity
{
    public string Name { get; set; }

    public string Subject { get; set; }

    public string Body { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public bool IsDeleted { get; set; }

    public List<EmailTemplateVariable> Variables { get; set; }

    public bool IsVisible { get; set; }

    public bool IsSystemDefined { get; set; }
}

public class EmailTemplateVariable : BaseEntity
{
    public string Name { get; set; }

    public string DefaultValue { get; set; }

    public Guid EmailTemplateId { get; set; }
}
