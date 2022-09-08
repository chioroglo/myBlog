using Domain.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DAL.Repositories.Abstract.Base
{
    public abstract class BaseRepository<TEntity> : IAsyncRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly BlogDbContext _db;

        protected BaseRepository(BlogDbContext db)
        {
            _db = db;
        }

        public async Task Add(TEntity entity)
        {
            await _db.Set<TEntity>().AddAsync(entity);
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await _db.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> GetById(int id)
        {
            return await _db.Set<TEntity>().FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetWhere(Expression<Func<TEntity, bool>> predicate)
        {
            return await _db.Set<TEntity>().Where(predicate).ToListAsync();
        }

        public async Task<bool> Remove(int id)
        {
            TEntity? entity = await GetById(id);
            try
            {

                if (entity != null)
                {
                    _db.Remove(entity);
                    return true;
                }
                else
                {
                    throw new ArgumentNullException($"could not found entity under id {id}");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine($"[BaseRepository/Remove] : {e.Message}");
                return false;
            }
        }


        public async Task Update(TEntity entity)
        {
            _db.Update(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
