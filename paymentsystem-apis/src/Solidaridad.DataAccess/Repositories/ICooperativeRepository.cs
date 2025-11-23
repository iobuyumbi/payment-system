

using Solidaridad.Core.Entities;

namespace Solidaridad.DataAccess.Repositories;

public interface ICooperativeRepository : IBaseRepository<Cooperative>
{
    int GetCount(Guid countryId);
}
