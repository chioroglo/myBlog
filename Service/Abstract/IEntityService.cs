using Domain.Models.Pagination;

namespace Service.Abstract
{
    public interface IEntityService<TEntity> where TEntity: class
    {
        Task Add(TEntity entity, CancellationToken cancellationToken);

        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken);

        Task<TEntity> GetByIdAsync(int id, CancellationToken cancellationToken);
        
        Task RemoveAsync(int id,int issuerId, CancellationToken cancellationToken);
        
        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);

        Task<PaginatedResult<TEntity>> GetPageAsync(PagedRequest query, CancellationToken cancellationToken);
    }
}