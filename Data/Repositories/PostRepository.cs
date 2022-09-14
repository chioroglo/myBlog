using DAL.Repositories.Abstract;
using DAL.Repositories.Abstract.Base;
using Domain;

namespace DAL.Repositories
{
    public class PostRepository : BaseRepository<Post>, IPostRepository
    {
        public PostRepository(BlogDbContext db) : base(db)
        {

        }

        public async Task<Post> GetByTitle(string title)
        {
            var found = await GetWhereAsync(e => e.Title == title); 

            return found.FirstOrDefault();
        }
    }
}
