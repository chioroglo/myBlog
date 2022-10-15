using System.Linq.Expressions;

namespace Service.Abstract
{
    public interface IEntityService<TEntity> where TEntity: class
    {
        Task Add(TEntity entity, CancellationToken cancellationToken);

        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken);

        Task<TEntity> GetByIdAsync(int id, CancellationToken cancellationToken);

        Task<TEntity> GetByIdWithIncludeAsync(int id, CancellationToken cancellationToken, params Expression<Func<TEntity, object>>[] includeProperties);
        
        Task RemoveAsync(int id,int issuerId, CancellationToken cancellationToken);
        
        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);

    }
}