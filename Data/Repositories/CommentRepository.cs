using DAL.Repositories.Abstract;
using DAL.Repositories.Abstract.Base;
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
