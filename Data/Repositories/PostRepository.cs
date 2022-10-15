using Common.Models.Pagination;
using DAL.Repositories.Abstract;
using DAL.Repositories.Abstract.Base;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class PostRepository : BaseRepository<Post>, IPostRepository
    {
        public PostRepository(BlogDbContext db) : base(db)
        {

        }

        public async Task<Post?> GetByTitleAsync(string title, CancellationToken cancellationToken)
        {
            var found = await _db.Posts.Where(e => e.Title == title).FirstOrDefaultAsync(cancellationToken); 

            return found;
        }
    }
}
