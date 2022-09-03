using System.Linq.Expressions;

namespace Service.Abstract
{
    public interface IBaseService<TEntity>
    {
        public Task Add(TEntity entity);

        public Task<IEnumerable<TEntity>> GetAll();

        public Task<TEntity> GetById(int id);
        
        public Task<bool> Remove(int id);
        
        public Task<bool> Update(TEntity entity);
    }
}