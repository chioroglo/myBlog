using DAL.Repositories.Abstract;
using Entities;

namespace DAL.Repositories
{
    public class UserRepository : BaseRepository<UserEntity>, IUserRepository
    {
        public UserRepository(BlogDbContext db) : base(db)
        {

        }
    }
}
