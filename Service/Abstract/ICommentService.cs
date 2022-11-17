using Common.Dto.Paging.CursorPaging;
using Domain;
using System.Linq.Expressions;

namespace Service.Abstract
{
    public interface ICommentService : IEntityService<Comment>
    {
        public Task<IEnumerable<Comment>> GetCommentsByPostId(int postId, CancellationToken cancellationToken);

        Task<CursorPagedResult<Comment>> GetCursorPageAsync(CursorPagedRequest query, CancellationToken cancellationToken, params Expression<Func<Comment, object>>[] includeProperties);
    }
}
