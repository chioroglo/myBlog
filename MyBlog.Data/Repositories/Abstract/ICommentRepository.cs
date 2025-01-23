using MyBlog.Data.Repositories.Abstract.Base;
using MyBlog.Domain;

namespace MyBlog.Data.Repositories.Abstract
{
    public interface ICommentRepository : IBaseRepository<Comment>
    {
        Task<IEnumerable<Comment>> GetByPostIdIncludeUserAndPostAsync(int postId, CancellationToken cancellationToken);
    }
}