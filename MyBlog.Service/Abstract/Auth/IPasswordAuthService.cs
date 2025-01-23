using MyBlog.Common.Dto.Auth;

namespace MyBlog.Service.Abstract.Auth
{
    public interface IPasswordAuthService
    {
        Task<AuthorizationResponse> AuthenticateAsync(PasswordAuthorizeRequest userData, CancellationToken cancellationToken);
    }
}