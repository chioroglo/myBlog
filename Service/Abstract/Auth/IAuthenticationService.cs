using Domain.Dto.Account;
using Domain.Dto.Auth;

namespace Service.Abstract.Auth
{
    public interface IAuthenticationService
    {
        public Task<AuthenticateResponse> TryIdentifyUser(string username, string password);

        public Task<AuthenticateResponse> Authenticate(AuthenticateRequest userData);
    }
}