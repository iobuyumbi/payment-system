using Solidaridad.Core.Entities;
using Solidaridad.DataAccess.Persistence;

namespace Solidaridad.DataAccess.Repositories.Impl;

public class AttachmentMappingRepository : BaseRepository<AttachmentMapping>, IAttachmentMappingRepository
{
    public AttachmentMappingRepository(DatabaseContext context) : base(context)
    {
    }
}
