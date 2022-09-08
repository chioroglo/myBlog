using DAL.Repositories.Abstract;
using DAL.Repositories.Abstract.Base;
using Domain;

namespace DAL.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(BlogDbContext db) : base(db)
        {

        }
    }
}
