using DAL.Repositories.Abstract.Base;
using Domain;

namespace DAL.Repositories.Abstract
{
    public interface IPostRepository : IAsyncRepository<Post>
    {
        Task<Post> GetByTitle(string title);
    }
}
