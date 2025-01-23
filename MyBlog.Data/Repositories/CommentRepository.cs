using Microsoft.EntityFrameworkCore;
using MyBlog.Data.Repositories.Abstract;
using MyBlog.Data.Repositories.Abstract.Base;
using MyBlog.Domain;

namespace MyBlog.Data.Repositories
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