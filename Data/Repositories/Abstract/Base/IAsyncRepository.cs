using Domain.Abstract;
using System.Linq.Expressions;

namespace DAL.Repositories.Abstract.Base
{
    public interface IAsyncRepository<T> where T : BaseEntity
    {
        Task SaveChangesAsync();
        // crud repo interface

        Task AddAsync(T entity);

        Task<T> GetByIdAsync(int id);

        Task<IEnumerable<T>> GetAllAsync();

        Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> predicate);

        Task<T> GetByIdWithIncludeAsync(int id, params Expression<Func<T, object>>[] includeProperties);

        Task UpdateAsync(T entity);

        Task<bool> RemoveAsync(int id);
    }
}
