using Solidaridad.Core.Entities;
using Solidaridad.DataAccess.Persistence;

namespace Solidaridad.DataAccess.Repositories.Impl;

public class CallBackRepository : BaseRepository<CallBackRecords>, ICallBackRepository
{
    public CallBackRepository(DatabaseContext context) : base(context)
    {
    }
}
