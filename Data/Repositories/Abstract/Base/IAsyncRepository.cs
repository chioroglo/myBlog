using Domain.Abstract;
using Domain.Models.Pagination;
using System.Linq.Expressions;

namespace DAL.Repositories.Abstract.Base
{
    public interface IAsyncRepository<TEntity> where TEntity : BaseEntity
    {
        Task SaveChangesAsync();
        // crud repo interface

        Task AddAsync(TEntity entity);

        Task<TEntity> GetByIdAsync(int id);

        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<IEnumerable<TEntity>> GetWhereAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> GetByIdWithIncludeAsync(int id, params Expression<Func<TEntity, object>>[] includeProperties);

        Task UpdateAsync(TEntity entity);

        Task<bool> RemoveAsync(int id);

        Task<PaginatedResult<TEntity>> GetPagedData(PagedRequest pagedRequest);
    }
}
