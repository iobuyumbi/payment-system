using Solidaridad.Core.Entities.Loans;

namespace Solidaridad.DataAccess.Repositories;

public interface IItemUnitRepository
{
    List<ItemUnit> GetItemUnits();
}
