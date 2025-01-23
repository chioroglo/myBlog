using MyBlog.Data.Repositories.Abstract.Base;
using MyBlog.Domain;

namespace MyBlog.Data.Repositories.Abstract
{
    public interface IAvatarRepository : IBaseRepository<Avatar>
    {
        Task<Avatar?> GetByUserIdAsync(int userId, CancellationToken cancellationToken);
        Task<bool> HasAvatar(int userId, CancellationToken ct);
    }
}