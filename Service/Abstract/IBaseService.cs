using Domain.Abstract;
using System.Linq.Expressions;

namespace Service.Abstract
{
    public interface IBaseService<TEntity>
    {
        public Task Add(TEntity entity);

        public Task<IEnumerable<TEntity>> GetAll();

        public Task<TEntity> GetById(int id);
        
        public Task<IEnumerable<TEntity>> GetWhere(Expression<Func<TEntity, bool>> predicate);
        
        public Task<bool> Remove(int id);
        
        public Task Update(TEntity entity);
    }
}
