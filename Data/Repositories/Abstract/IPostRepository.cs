using DAL.Repositories.Abstract.Base;
using Domain;

namespace DAL.Repositories.Abstract
{
    public interface IPostRepository : IBaseRepository<Post>
    {
        Task<Post?> GetByTitleAsync(string title, CancellationToken cancellationToken);
    }
}