using Microsoft.EntityFrameworkCore;
using MyBlog.Data.Repositories.Abstract;
using MyBlog.Data.Repositories.Abstract.Base;
using MyBlog.Domain;

namespace MyBlog.Data.Repositories;

public class EfPasskeyRepository : BaseRepository<Passkey>, IPasskeyRepository
{
    public EfPasskeyRepository(BlogDbContext db) : base(db)
    {

    }

    public async Task<User?> GetUserWithActivePasskeys(int userId, CancellationToken ct)
    {
        return await _db.Set<User>()
            .Include(u => u.Passkeys
                .Where(p => p.IsActive))
            .FirstOrDefaultAsync(u => u.Id == userId, ct);
    }
}