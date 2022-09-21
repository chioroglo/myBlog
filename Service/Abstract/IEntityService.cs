using Domain.Models.Pagination;

namespace Service.Abstract
{
    public interface IEntityService<TEntity> where TEntity: class
    {
        Task Add(TEntity entity);

        Task<IEnumerable<TEntity>> GetAll();

        Task<TEntity> GetById(int id);
        
        Task Remove(int id,int issuerId);
        
        Task Update(TEntity entity);

        Task<PaginatedResult<TEntity>> GetPage(PagedRequest query);
    }
}