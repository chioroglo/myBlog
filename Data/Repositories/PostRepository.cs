using DAL.Repositories.Abstract;
using DAL.Repositories.Abstract.Base;
using Domain;

namespace DAL.Repositories
{
    public class PostRepository : BaseRepository<PostEntity>, IPostRepository
    {
        public PostRepository(BlogDbContext db) : base(db)
        {
        }
    }
}
