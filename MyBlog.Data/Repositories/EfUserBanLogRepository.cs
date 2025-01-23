using MyBlog.Data.Repositories.Abstract;
using MyBlog.Data.Repositories.Abstract.Base;
using MyBlog.Domain;

namespace MyBlog.Data.Repositories;

public class EfUserBanLogRepository : BaseRepository<UserBanLog>, IUserBanLogRepository
{
    public EfUserBanLogRepository(BlogDbContext db) : base(db)
    {
    }
}