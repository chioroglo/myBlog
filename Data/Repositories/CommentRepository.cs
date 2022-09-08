using DAL.Repositories.Abstract;
using DAL.Repositories.Abstract.Base;
using Domain;

namespace DAL.Repositories
{
    public class CommentRepository : BaseRepository<Comment>, ICommentRepository
    {
        public CommentRepository(BlogDbContext db) : base(db)
        {

        }
    }
}