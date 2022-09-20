using DAL.Repositories.Abstract;
using DAL.Repositories.Abstract.Base;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class AvatarRepository : BaseRepository<Avatar>, IAvatarRepository
    {
        public AvatarRepository(BlogDbContext db) : base(db)
        {

        }

        public async Task<Avatar?> GetByUserIdAsync(int userId)
        {
            var avatarInfo =  await _db.Avatars.Where(a => a.UserId == userId).FirstOrDefaultAsync();

            return avatarInfo;
        }
    }
}
