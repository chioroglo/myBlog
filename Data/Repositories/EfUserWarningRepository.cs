using DAL.Repositories.Abstract;
using DAL.Repositories.Abstract.Base;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

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