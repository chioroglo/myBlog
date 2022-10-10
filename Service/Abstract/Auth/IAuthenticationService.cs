using Common.Dto.Auth;

namespace Service.Abstract.Auth
{
    public interface IAuthenticationService
    {
        Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest userData,CancellationToken cancellationToken);
    }
}