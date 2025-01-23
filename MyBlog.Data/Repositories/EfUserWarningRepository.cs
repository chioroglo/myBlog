using Microsoft.EntityFrameworkCore;
using MyBlog.Data.Repositories.Abstract;
using MyBlog.Data.Repositories.Abstract.Base;
using MyBlog.Domain;

namespace MyBlog.Data.Repositories;

public class EfUserWarningRepository : BaseRepository<UserWarning>, IUserWarningRepository
{
    public EfUserWarningRepository(BlogDbContext db) : base(db) {}

    public async Task<int> GetUserActiveWarningsCount(int userId, CancellationToken ct = default)
    {
        return await _db.UserWarnings
            .Where(uw => uw.RemovedAt == null && uw.UserId == userId)
            .CountAsync(ct);
    }
}