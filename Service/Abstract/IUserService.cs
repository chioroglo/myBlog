using Domain;

namespace Service.Abstract
{
    public interface IUserService : IBaseService<User>
    {
        public Task<User> GetByUsername(string username);
    }
}
