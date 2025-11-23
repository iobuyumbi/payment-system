using System.Linq.Expressions;
using Solidaridad.Core.Common;

namespace Solidaridad.DataAccess.Repositories;

public interface IBaseRepository<TEntity> where TEntity : BaseEntity
{
    Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> predicate);

    Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate);

    Task<TEntity> AddAsync(TEntity entity);

    Task<TEntity> UpdateAsync(TEntity entity);

    Task<TEntity> DeleteAsync(TEntity entity);

    Task AddRange(IEnumerable<TEntity> entities);

    Task UpdateRange(IEnumerable<TEntity> entities);
}
