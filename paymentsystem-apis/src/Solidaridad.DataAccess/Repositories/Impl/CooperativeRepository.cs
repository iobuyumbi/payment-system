
using Microsoft.EntityFrameworkCore;
using Solidaridad.Core.Entities;
using Solidaridad.DataAccess.Persistence;

namespace Solidaridad.DataAccess.Repositories.Impl;

public class CooperativeRepository : BaseRepository<Cooperative>, ICooperativeRepository
{
    #region DI
    protected readonly DbSet<Cooperative> copSet;

    public CooperativeRepository(DatabaseContext context) : base(context)
    {
        copSet = context.Set<Cooperative>();
    }
    #endregion

    #region Methods
    public int GetCount(Guid countryId)
    {
        return copSet.Count(c=> c.CountryId == countryId && !c.IsDeleted);
    }
    #endregion
}
