using Domain;
using Domain.Dto.Account;

namespace Service.Abstract.Auth
{
    public interface IRegistrationService
    {
        public Task<UserModel> Register(RegistrationDto registerData);
    }
}
