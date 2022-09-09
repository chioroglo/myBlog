using Domain;

namespace Service.Abstract
{
    public interface ICommentService : IBaseService<Comment>
    {
        public Task<IEnumerable<Comment>> GetCommentsByPostId(int postId);
    }
}
