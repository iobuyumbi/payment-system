using Microsoft.EntityFrameworkCore;
using Solidaridad.Core.Entities.Loans;
using Solidaridad.DataAccess.Persistence;

namespace Solidaridad.DataAccess.Repositories.Impl;

public class ItemUnitRepository : IItemUnitRepository
{
    protected readonly DbSet<ItemUnit> itemUnit;
    public ItemUnitRepository(DatabaseContext context)
    {
        itemUnit = context.Set<ItemUnit>();
    }

    public List<ItemUnit> GetItemUnits()
    {
        return itemUnit.Where(x => x != null).ToList();
    }
}
