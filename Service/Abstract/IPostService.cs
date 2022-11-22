using Common.Dto.Paging.CursorPaging;
using Domain;
using System.Linq.Expressions;

namespace Service.Abstract
{
    public interface IPostService : IEntityService<Post>
    {
        Task<CursorPagedResult<Post>> GetCursorPageAsync(CursorPagedRequest query, CancellationToken cancellationToken,
            params Expression<Func<Post, object>>[] includeProperties);
    }
}