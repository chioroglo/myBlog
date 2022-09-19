using Domain.Abstract;
using Domain.Exceptions;
using Domain.Extensions;
using Domain.Models.Pagination;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DAL.Repositories.Abstract.Base
{
    public abstract class BaseRepository<TEntity> : IAsyncRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly BlogDbContext _db;

        protected BaseRepository(BlogDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(TEntity entity)
        {
            await _db.Set<TEntity>().AddAsync(entity);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _db.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            var entity = await _db.Set<TEntity>().FindAsync(id);

            if (entity == null)
            {
                throw new ValidationException($"{typeof(TEntity).Name} of ID: {id} does not exist");
            }

            return entity;
        }

        public async Task<IEnumerable<TEntity>> GetWhereAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _db.Set<TEntity>().Where(predicate).ToListAsync();
        }

        public async Task<bool> RemoveAsync(int id)
        {
            TEntity? entity = await GetByIdAsync(id);
            
            _db.Remove(entity);

            return true;
        }


        public async Task UpdateAsync(TEntity entity)
        {

            // refactor this method
            _db.Update(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task<TEntity> GetByIdWithIncludeAsync(int id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            // refactor this method, add exception
            var query = IncludeProperties(includeProperties);

            return await query.FirstOrDefaultAsync(e => e.Id == id);
        }
        public async Task<PaginatedResult<TEntity>> GetPagedData(PagedRequest pagedRequest)
        {
            return await _db.Set<TEntity>().CreatePaginatedResultAsync(pagedRequest);
        }

        private IQueryable<TEntity> IncludeProperties(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> entities = _db.Set<TEntity>();
            
            foreach(var includeProperty in includeProperties)
            {
                entities = entities.Include(includeProperty);
            }

            return entities;
        }

    }
}
