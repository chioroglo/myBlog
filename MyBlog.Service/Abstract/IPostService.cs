using System.Linq.Expressions;
using MyBlog.Common.Dto.Paging.CursorPaging;
using MyBlog.Domain;

namespace MyBlog.Service.Abstract
{
    public interface IPostService : IEntityService<Post>
    {
        Task<CursorPagedResult<Post>> GetCursorPageAsync(CursorPagedRequest query, CancellationToken cancellationToken,
            params Expression<Func<Post, object>>[] includeProperties);
    }
}