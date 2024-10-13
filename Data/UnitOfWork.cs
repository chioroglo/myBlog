using Domain.Abstract;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class UnitOfWork(DbContext context) : IUnitOfWork
{
    public async Task<int> CommitAsync(CancellationToken ct = default)
    {
        return await context.SaveChangesAsync(ct);
    }
}