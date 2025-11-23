using Microsoft.EntityFrameworkCore;
using Solidaridad.Core.Entities;
using Solidaridad.DataAccess.Migrations;
using Solidaridad.DataAccess.Persistence;

namespace Solidaridad.DataAccess.Repositories.Impl;

public class FarmerRepository : BaseRepository<Farmer>, IFarmerRepository
{
    #region DI
    protected readonly DbSet<Farmer> farmerSet;
    protected readonly DbSet<Project> projectSet;
    protected readonly DbSet<FarmerProject> farmerProjectSet;

    public FarmerRepository(DatabaseContext context) : base(context)
    {
        farmerSet = context.Set<Farmer>();
        projectSet = context.Set<Project>();
        farmerProjectSet = context.Set<FarmerProject>();
    }
    #endregion

    #region Methods
    public int GetCount(Guid countryId)
    {
        return farmerSet.Count(c=> c.CountryId == countryId && !c.IsDeleted);
    }

    public async Task<List<FarmerProject>> AddFarmerProjectAsync(Guid farmerId, List<Guid> projectIds)
    {
        var farmerProjects = projectIds.Select(projectId => new FarmerProject
        {
            FarmerId = farmerId,
            ProjectId = projectId
        }).ToList();

        await farmerProjectSet.AddRangeAsync(farmerProjects);
        await Context.SaveChangesAsync();

        return farmerProjects;
    }

    public async Task<List<FarmerProject>> UpdateFarmerProjectAsync(Guid farmerId, List<Guid> projectIds)
    {
        // Get distinct countryIds and ensure they exist
        var validProjectIds = await projectSet
            .Where(c => projectIds.Contains(c.Id))
            .Select(c => c.Id)
            .ToListAsync();

        // Avoid duplicates
        var existingMappings = await farmerProjectSet
            .Where(uc => uc.FarmerId == farmerId && validProjectIds.Contains(uc.ProjectId))
            .Select(uc => uc.ProjectId)
            .ToListAsync();

        var newCountryIds = validProjectIds.Except(existingMappings).ToList();

        var newFarmerProjects = newCountryIds.Select(pid => new FarmerProject
        {
            FarmerId = farmerId,
            ProjectId = pid
        }).ToList();

        if (newFarmerProjects.Any())
        {
            await farmerProjectSet.AddRangeAsync(newFarmerProjects);
            await Context.SaveChangesAsync();
        }

        return newFarmerProjects;
    }

    public async Task<IEnumerable<Project>> GetFarmerProjects(Guid farmerId)
    {
        var selectItems = from fp in farmerProjectSet
                          join p in projectSet on fp.ProjectId equals p.Id
                          join f in farmerSet on fp.FarmerId equals f.Id
                          where f.Id == farmerId
                          select new Project
                          {
                              ProjectName = p.ProjectName,
                              Id = p.Id
                          };

        return selectItems;
    }

    #endregion
}
