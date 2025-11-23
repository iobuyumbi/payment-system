using Solidaridad.Core.Entities.Email;
using Solidaridad.DataAccess.Persistence;

namespace Solidaridad.DataAccess.Repositories.Impl;

public class EmailTemplateRepository: BaseRepository<EmailTemplate>, IEmailTemplateRepository
{
    public EmailTemplateRepository(DatabaseContext context) : base(context) { }
}

