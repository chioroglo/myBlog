using Common.Models.Pagination;
using Domain.Abstract;
using System.Linq.Expressions;

namespace DAL.Repositories.Abstract.Base
{
    public interface IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity?> GetByIdAsync(int id,CancellationToken cancellationToken);

        Task<TEntity?> GetByIdWithIncludeAsync(int id, CancellationToken cancellationToken, params Expression<Func<TEntity, object>>[] includeProperties);
        
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken);

        Task<IEnumerable<TEntity>> GetWhereAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);

        Task<PaginatedResult<TEntity>> GetPagedData(PagedRequest pagedRequest, CancellationToken cancellationToken);
        
        Task AddAsync(TEntity entity, CancellationToken cancellationToken);

        void Update(TEntity entity, CancellationToken cancellationToken);

        /// <summary>
        /// This method is async only to retrieve entity of corresponding id asynchronously to ensure it is created
        /// </summary>
        Task RemoveAsync(int id, CancellationToken cancellationToken);
    }
}
