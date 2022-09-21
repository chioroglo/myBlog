using Domain;

namespace Service.Abstract
{
    public interface IUserService : IEntityService<User>
    {
        public Task<User?> GetByUsername(string username);
    }
}
