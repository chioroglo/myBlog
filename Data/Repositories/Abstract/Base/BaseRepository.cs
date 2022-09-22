using Domain.Abstract;
using Domain.Exceptions;
using Domain.Extensions;
using Domain.Models.Pagination;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DAL.Repositories.Abstract.Base
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly BlogDbContext _db;

        protected BaseRepository(BlogDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _db.Set<TEntity>().ToListAsync(cancellationToken);
        }

        public async Task<TEntity> GetByIdAsync(int id,CancellationToken cancellationToken)
        {
            var entity = await _db.Set<TEntity>().FindAsync(id,cancellationToken);

            return entity;
        }

        public async Task<IEnumerable<TEntity>> GetWhereAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _db.Set<TEntity>().Where(predicate).ToListAsync(cancellationToken);
        }

        public async Task RemoveAsync(int id, CancellationToken cancellationToken)
        {
            TEntity? entity = await GetByIdAsync(id,cancellationToken);

            if (entity == null)
            {
                throw new ValidationException($"{typeof(TEntity).Name} of ID: {id} does not exist");
            }

            _db.Remove(entity);
        }


        public void Update(TEntity entity,CancellationToken cancellationToken)
        {
            _db.Entry(entity).State = EntityState.Modified;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task<TEntity?> GetByIdWithIncludeAsync(int id, CancellationToken cancellationToken, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = IncludeProperties(includeProperties);

            return await query.FirstOrDefaultAsync(e => e.Id == id,cancellationToken);
        }
        public async Task<PaginatedResult<TEntity>> GetPagedData(PagedRequest pagedRequest, CancellationToken cancellationToken)
        {
            return await _db.Set<TEntity>().CreatePaginatedResultAsync(pagedRequest,cancellationToken);
        }

        public async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
        {
            await _db.Set<TEntity>().AddAsync(entity,cancellationToken);
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
