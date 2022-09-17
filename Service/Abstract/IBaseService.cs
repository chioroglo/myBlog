using Domain.Models.Pagination;

namespace Service.Abstract
{
    public interface IBaseService<TEntity> where TEntity: class
    {
        Task Add(TEntity entity);

        Task<IEnumerable<TEntity>> GetAll();

        Task<TEntity> GetById(int id);
        
        Task<bool> Remove(int id,int issuerId);
        
        Task<bool> Update(TEntity entity);

        Task<PaginatedResult<TEntity>> GetPage(PagedRequest query);
    }
}