using MyBlog.Data.Repositories.Abstract;
using MyBlog.Data.Repositories.Abstract.Base;
using MyBlog.Domain;

namespace MyBlog.Data.Repositories
{
    public class PostReactionRepository : BaseRepository<PostReaction>, IPostReactionRepository
    {
        public PostReactionRepository(BlogDbContext db) : base(db)
        {
        }
    }
}