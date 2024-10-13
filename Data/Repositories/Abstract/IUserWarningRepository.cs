using DAL.Repositories.Abstract.Base;
using Domain;

namespace DAL.Repositories.Abstract;

public interface IUserWarningRepository : IBaseRepository<UserWarning>
{
    Task<int> GetUserActiveWarningsCount(int userId, CancellationToken ct = default);
}