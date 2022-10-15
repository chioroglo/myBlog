using DAL.Repositories.Abstract;
using DAL.Repositories.Abstract.Base;
using Domain;

namespace DAL.Repositories
{
    public class TopicRepository : BaseRepository<Topic>, ITopicRepository
    {
        public TopicRepository(BlogDbContext db) : base(db)
        {

        }
    }
}
