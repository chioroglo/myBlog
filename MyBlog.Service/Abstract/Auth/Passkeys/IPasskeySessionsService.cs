using Fido2NetLib;
using MyBlog.Common.Models.Passkey;
using MyBlog.Domain;

namespace MyBlog.Service.Abstract.Auth.Passkeys;

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