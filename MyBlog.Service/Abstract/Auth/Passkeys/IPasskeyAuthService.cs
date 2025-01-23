using Common.Dto.Auth;
using MyBlog.Common.Dto.Auth;
using MyBlog.Common.Models.Passkey;

namespace MyBlog.Service.Abstract.Auth.Passkeys;

public interface IPasskeyAuthService
{
    Task<PasskeyRegistrationOptionsModel> GetOrCreateRegistrationSession(int userId, CancellationToken ct);
    Task Register(RegisterPasskeyRequest request, int userId, CancellationToken ct);
    Task<PasskeyAuthenticationOptionsModel> StartAuthenticationSession(CancellationToken ct);
    Task<AuthorizationResponse> Authenticate(AuthenticatePasskeyRequest request, CancellationToken ct);
    Task Deactivate(int passkeyId, int userId, CancellationToken ct);
}