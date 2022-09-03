using Domain;
using Domain.Dto.Account;

namespace Service.Abstract.Auth
{
    public interface IAuthenticationService
    {
        public Task<AuthenticateResponse> Authenticate(AuthenticateRequest userData);
    }
}