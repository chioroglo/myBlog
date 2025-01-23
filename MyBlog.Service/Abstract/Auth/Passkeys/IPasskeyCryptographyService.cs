using Common.Dto.Auth;
using Fido2NetLib;
using MyBlog.Common.Dto.Auth;
using MyBlog.Domain;

namespace MyBlog.Service.Abstract.Auth.Passkeys;

public interface IPasskeyCryptographyService
{
    Task<Passkey> ValidateRegistration(RegisterPasskeyRequest request, User user,
        CredentialCreateOptions options, CancellationToken ct);

    Task ValidateAuthentication(AuthenticatePasskeyRequest request, User user, AssertionOptions options, CancellationToken ct);
}