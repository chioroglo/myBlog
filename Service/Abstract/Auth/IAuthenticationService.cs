using Domain.Dto.Account;
using Domain.Dto.Auth;

namespace Service.Abstract.Auth
{
    public interface IAuthenticationService
    {
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest userData);
    }
}