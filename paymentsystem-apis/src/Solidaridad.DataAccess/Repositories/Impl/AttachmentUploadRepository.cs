using Solidaridad.Core.Entities;
using Solidaridad.DataAccess.Persistence;

namespace Solidaridad.DataAccess.Repositories.Impl;

public class AttachmentUploadRepository : BaseRepository<AttachmentFile>, IAttachmentUploadRepository
{
    public AttachmentUploadRepository(DatabaseContext context) : base(context)
    {
    }
}
