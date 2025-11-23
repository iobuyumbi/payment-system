using Solidaridad.Core.Entities.Loans;
using Solidaridad.DataAccess.Persistence;

namespace Solidaridad.DataAccess.Repositories.Impl;

public class ItemCategoryRepository : BaseRepository<ItemCategory>, IItemCategoryRepository
{
    public ItemCategoryRepository(DatabaseContext context) : base(context) { }
}
