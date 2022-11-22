using Common.Dto.Auth;
using Domain;

namespace Service.Abstract.Auth
{
    public interface IRegistrationService
    {
        public Task<User> RegisterAsync(RegistrationDto registerData, CancellationToken cancellationToken);
    }
}