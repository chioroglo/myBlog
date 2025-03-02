using MyBlog.Common.Dto.Auth;

namespace MyBlog.Service.Abstract.Auth
{
    public interface IPasswordAuthService
    {
        Task<AuthorizationResponse> AuthenticateAsync(PasswordAuthorizeRequest userData, CancellationToken cancellationToken);
        /// <summary>
        /// Changes password of specific user, validating his credentials
        /// </summary>
        /// <returns>Refresh and access token pair</returns>
        Task<AuthorizationResponse> ChangePasswordAsync(ChangePasswordDto dto, CancellationToken ct = default);
    }
}