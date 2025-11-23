using Solidaridad.Core.Entities.Email;
using Solidaridad.DataAccess.Persistence;

namespace Solidaridad.DataAccess.Repositories.Impl;

public class EmailTemplateVariableRepository : BaseRepository<EmailTemplateVariable>, IEmailTemplateVariableRepository
{
    public EmailTemplateVariableRepository(DatabaseContext context) : base(context) { }
}

