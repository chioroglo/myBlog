using Common.Dto.Auth;
using Domain;
using Fido2NetLib;

namespace Service.Abstract.Auth.Passkeys;

public interface IPasskeyCryptographyService
{
    Task<Passkey> ValidateRegistration(RegisterPasskeyRequest request, User user,
        CredentialCreateOptions options, CancellationToken ct);

    Task ValidateAuthentication(AuthenticatePasskeyRequest request, User user, AssertionOptions options, CancellationToken ct);
}