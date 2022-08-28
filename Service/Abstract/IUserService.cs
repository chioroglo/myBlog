using Domain.Dto.Account;
using Entities;

namespace Service.Abstract
{
    public interface IUserService : IBaseService<UserEntity>
    {
        public Task<AuthenticateResponse> TryIdentifyUser(string username, string password);

        public Task<AuthenticateResponse> GetCurrentUser();
    }
}
