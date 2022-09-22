using Domain;

namespace Service.Abstract
{
    public interface ICommentService : IEntityService<Comment>
    {
        public Task<IEnumerable<Comment>> GetCommentsByPostId(int postId, CancellationToken cancellationToken);
    }
}
