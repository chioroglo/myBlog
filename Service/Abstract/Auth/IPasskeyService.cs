using Common.Dto.Auth;
using Fido2NetLib;
using Fido2NetLib.Development;

namespace Service.Abstract.Auth;

public interface IPasskeyService
{
    Task<CredentialCreateOptions> GetCredentialOptionsAsync(int userId, CancellationToken ct);
    Task<Fido2.CredentialMakeResult> MakeCredential(AuthenticatorAttestationRawResponse dto, int userId,
        CancellationToken ct);
    Task<AssertionOptions> GetAuthenticationOptionsAsync(int userId, CancellationToken ct);
    Task<AuthenticateResponse> Authenticate(int userId,AuthenticatorAssertionRawResponse dto , CancellationToken ct);
}