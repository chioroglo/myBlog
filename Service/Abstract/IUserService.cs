using Domain;
using Domain.Dto.Account;
using Domain.Models;

namespace Service.Abstract
{
    public interface IUserService : IBaseService<User>
    {
        
        public Task<UserModel> GetByUsername(string username);
    }
}
