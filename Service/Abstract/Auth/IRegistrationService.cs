using Domain.Dto.Account;
using Domain.Models;

namespace Service.Abstract.Auth
{
    public interface IRegistrationService
    {
        public Task<UserModel> RegisterAsync(RegistrationDto registerData, CancellationToken cancellationToken);
    }
}
