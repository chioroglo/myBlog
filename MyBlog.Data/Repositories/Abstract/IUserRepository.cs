using MyBlog.Common.Models;
using MyBlog.Data.Repositories.Abstract.Base;
using MyBlog.Domain;

namespace MyBlog.Data.Repositories.Abstract;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> GetProfileData(int userId, CancellationToken ct = default);
    Task<bool> IsBanned(int userId, CancellationToken ct = default);
    Task<bool> IsNicknameOccupied(string username, CancellationToken ct = default);
    Task<User?> GetUserByCredentials(string username, string passwordHash, CancellationToken ct = default);
    Task<UserBadgeModel?> GetBadge(int userId, CancellationToken ct = default);
}