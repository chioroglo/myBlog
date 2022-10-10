using Common.Dto.Auth;

namespace Service.Abstract.Auth
{
    public interface IRegistrationService
    {
        public Task RegisterAsync(RegistrationDto registerData, CancellationToken cancellationToken);
    }
}
