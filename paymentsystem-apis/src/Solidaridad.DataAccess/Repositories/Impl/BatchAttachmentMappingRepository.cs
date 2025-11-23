using Solidaridad.Core.Entities;
using Solidaridad.DataAccess.Persistence;

namespace Solidaridad.DataAccess.Repositories.Impl;

public class BatchAttachmentMappingRepository : BaseRepository<BatchAttachmentMapping>, IBatchAttachmentMappingRepository
{
    public BatchAttachmentMappingRepository(DatabaseContext context) : base(context)
    {
    }
}
