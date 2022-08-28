using DAL.Repositories.Abstract;
using Entities;

namespace DAL.Repositories
{
    public class CommentRepository : BaseRepository<CommentEntity>, ICommentRepository
    {
        public CommentRepository(BlogDbContext db) : base(db)
        {

        }
    }
}
