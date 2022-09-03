using Domain;
using Domain.Dto.Account;
using Entities;

namespace Service.Abstract
{
    public interface IUserService : IBaseService<UserEntity>
    {
        
        public Task<UserModel> GetByUsername(string username);
    }
}
