using Solidaridad.Core.Entities;
using Solidaridad.DataAccess.Persistence;

namespace Solidaridad.DataAccess.Repositories.Impl;

public class EMIScheduleRepository : BaseRepository<EMISchedule>, IEMIScheduleRepository
{
    public EMIScheduleRepository(DatabaseContext context) : base(context) { }
}
