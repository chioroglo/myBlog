using DAL.Repositories.Abstract;
using DAL.Repositories.Abstract.Base;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class CommentRepository : BaseRepository<Comment>, ICommentRepository
    {
        public CommentRepository(BlogDbContext db) : base(db)
        {
        }

        public async Task<IEnumerable<Comment>> GetByPostIdIncludeUserAndPostAsync(int postId,
            CancellationToken cancellationToken)
        {
            return await _db.Comments.Include(property => property.User).Include(property => property.Post)
                .Where(e => e.PostId == postId).ToListAsync(cancellationToken);
        }
    }
}