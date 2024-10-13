using DAL.Repositories.Abstract;
using DAL.Repositories.Abstract.Base;
using Domain;

namespace DAL.Repositories;

public class EfUserBanLogRepository : BaseRepository<UserBanLog>, IUserBanLogRepository
{
    public EfUserBanLogRepository(BlogDbContext db) : base(db)
    {
    }
}