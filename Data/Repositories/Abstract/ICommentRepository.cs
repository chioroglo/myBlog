using DAL.Repositories.Abstract.Base;
using Domain;

namespace DAL.Repositories.Abstract
{
    public interface ICommentRepository : IBaseRepository<Comment>
    {
        Task<IEnumerable<Comment>> GetByPostIdIncludeUserAndPostAsync(int postId, CancellationToken cancellationToken);
    }
}
