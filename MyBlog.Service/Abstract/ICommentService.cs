using System.Linq.Expressions;
using MyBlog.Common.Dto.Paging.CursorPaging;
using MyBlog.Domain;

namespace MyBlog.Service.Abstract
{
    public interface ICommentService : IEntityService<Comment>
    {
        public Task<IEnumerable<Comment>> GetCommentsByPostId(int postId, CancellationToken cancellationToken);

        Task<CursorPagedResult<Comment>> GetCursorPageAsync(CursorPagedRequest query,
            CancellationToken cancellationToken, params Expression<Func<Comment, object>>[] includeProperties);
    }
}