using DAL.Repositories.Abstract;
using DAL.Repositories.Abstract.Base;
using Domain;

namespace DAL.Repositories
{
    public class PostReactionRepository : BaseRepository<PostReaction>, IPostReactionRepository
    {
        public PostReactionRepository(BlogDbContext db) : base(db)
        {

        }
    }
}
