namespace Domain.Abstract;

public interface IUnitOfWork
{
    Task<int> CommitAsync(CancellationToken ct = default);
}