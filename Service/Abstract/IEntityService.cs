using Domain.Models.Pagination;

namespace Service.Abstract
{
    public interface IEntityService<TEntity> where TEntity: class
    {
        Task Add(TEntity entity, CancellationToken cancellationToken);

        Task<IEnumerable<TEntity>> GetAll(CancellationToken cancellationToken);

        Task<TEntity> GetById(int id, CancellationToken cancellationToken);
        
        Task Remove(int id,int issuerId, CancellationToken cancellationToken);
        
        Task Update(TEntity entity, CancellationToken cancellationToken);

        Task<PaginatedResult<TEntity>> GetPage(PagedRequest query, CancellationToken cancellationToken);
    }
}