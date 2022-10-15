using Common.Dto.Paging.OffsetPaging;
using Domain;
using System.Linq.Expressions;

namespace Service.Abstract
{
    public interface ICommentService : IEntityService<Comment>
    {
        public Task<IEnumerable<Comment>> GetCommentsByPostId(int postId, CancellationToken cancellationToken);

        Task<OffsetPagedResult<Comment>> GetOffsetPageAsync(OffsetPagedRequest query, CancellationToken cancellationToken, params Expression<Func<Comment, object>>[] includeProperties);
    }
}
