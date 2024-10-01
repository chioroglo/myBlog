using Common.Models.Passkey;
using Domain;
using Fido2NetLib;

namespace Service.Abstract.Auth.Passkeys;

public interface IPasskeySessionsService
{
    // Registration
    Task<CredentialCreateOptions?> GetOngoingRegistrationSession(int userId, CancellationToken ct);
    Task<CredentialCreateOptions> CreateRegistrationSession(User user, CancellationToken ct);
    Task RemoveOngoingRegistrationSession(int userId, CancellationToken ct);
    // Authentication
    Task<AssertionOptions?> GetOngoingAuthenticationSession(string challenge, CancellationToken ct);
    Task<PasskeyAuthenticationOptionsModel> CreateAuthenticationSession(CancellationToken ct);
    Task RemoveOngoingAuthenticationSession(string challenge, CancellationToken ct);
}