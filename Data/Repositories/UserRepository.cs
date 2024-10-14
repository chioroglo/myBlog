using DAL.Repositories.Abstract;
using DAL.Repositories.Abstract.Base;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(BlogDbContext db) : base(db)
    {
    }

    public async Task<bool> IsBanned(int userId, CancellationToken ct = default)
    {
        return await _db.Users.Where(u => u.Id == userId && u.IsBanned).CountAsync(ct) > 0;
    }
}