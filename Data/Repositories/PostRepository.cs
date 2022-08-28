using DAL.Repositories.Abstract;
using Entities;

namespace DAL.Repositories
{
    public class PostRepository : BaseRepository<PostEntity>, IPostRepository
    {
        public PostRepository(BlogDbContext db) : base(db)
        {
        }
    }
}
