using DAL.Repositories.Abstract.Base;
using Domain;

namespace DAL.Repositories.Abstract;

public interface IUserRepository : IBaseRepository<User>
{
    Task<bool> IsBanned(int userId, CancellationToken ct = default);
}