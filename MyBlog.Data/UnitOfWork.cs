using Microsoft.EntityFrameworkCore;
using MyBlog.Domain.Abstract;

namespace MyBlog.Data;

public class UnitOfWork(DbContext context) : IUnitOfWork
{
    public async Task<int> CommitAsync(CancellationToken ct = default)
    {
        return await context.SaveChangesAsync(ct);
    }
}