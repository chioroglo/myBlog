using Entities.Abstract;
using System.Linq.Expressions;

namespace DAL.Repositories.Abstract.Base
{
    public interface IAsyncRepository<T> where T : BaseEntity
    {
        // crud repo interface

        Task Add(T entity);

        Task<T> GetById(int id);

        Task<IEnumerable<T>> GetAll();

        Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate);

        Task Update(T entity);

        Task<bool> Remove(int id);
    }
}
