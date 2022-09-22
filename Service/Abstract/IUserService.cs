using Domain;

namespace Service.Abstract
{
    public interface IUserService : IEntityService<User>
    {
        Task<User?> GetByUsername(string username, CancellationToken cancellationToken);
    }
}
