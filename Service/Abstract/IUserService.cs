using Domain;
using Domain.Dto.Account;
using Domain.Models;

namespace Service.Abstract
{
    public interface IUserService : IBaseService<UserEntity>
    {
        
        public Task<UserModel> GetByUsername(string username);
    }
}
