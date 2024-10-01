using DAL.Repositories.Abstract.Base;
using Domain;

namespace DAL.Repositories.Abstract;

public interface IPasskeyRepository : IBaseRepository<Passkey>
{
    Task<User?> GetUserWithActivePasskeys(int userId, CancellationToken ct);
}