using MyBlog.Data.Repositories.Abstract.Base;
using MyBlog.Domain;

namespace MyBlog.Data.Repositories.Abstract;

public interface IPasskeyRepository : IBaseRepository<Passkey>
{
    Task<User?> GetUserWithActivePasskeys(int userId, CancellationToken ct);
}