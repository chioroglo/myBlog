using MyBlog.Data.Repositories.Abstract.Base;
using MyBlog.Domain;

namespace MyBlog.Data.Repositories.Abstract;

public interface IUserWarningRepository : IBaseRepository<UserWarning>
{
    Task<int> GetUserActiveWarningsCount(int userId, CancellationToken ct = default);
}