using Domain.Models.Pagination;

namespace Service.Abstract
{
    // todo remove bool from remove and update, adjust classes implementing this interface
    public interface IEntityService<TEntity> where TEntity: class
    {
        Task Add(TEntity entity);

        Task<IEnumerable<TEntity>> GetAll();

        Task<TEntity> GetById(int id);
        
        Task<bool> Remove(int id,int issuerId);
        
        Task<bool> Update(TEntity entity);

        Task<PaginatedResult<TEntity>> GetPage(PagedRequest query);
    }
}