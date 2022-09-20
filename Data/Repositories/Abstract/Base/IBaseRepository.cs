using Domain.Abstract;
using Domain.Models.Pagination;
using System.Linq.Expressions;

namespace DAL.Repositories.Abstract.Base
{
    public interface IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        Task SaveChangesAsync();

        Task<TEntity?> GetByIdAsync(int id);

        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<IEnumerable<TEntity>> GetWhereAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity?> GetByIdWithIncludeAsync(int id, params Expression<Func<TEntity, object>>[] includeProperties);

        Task<PaginatedResult<TEntity>> GetPagedData(PagedRequest pagedRequest);
        
        void Add(TEntity entity);

        void Update(TEntity entity);

        /// <summary>
        /// This method is async only to retrieve entity of corresponding id asynchronously
        /// </summary>
        Task RemoveAsync(int id);
    }
}
