using Solidaridad.Core.Entities;

namespace Solidaridad.DataAccess.Repositories;

public interface IFarmerRepository : IBaseRepository<Farmer>
{
    Task<List<FarmerProject>> AddFarmerProjectAsync(Guid farmerId, List<Guid> projectIds);
    
    int GetCount(Guid countryId);
    
    Task<IEnumerable<Project>> GetFarmerProjects(Guid farmerId);

    Task<List<FarmerProject>> UpdateFarmerProjectAsync(Guid farmerId, List<Guid> projectIds);
}
