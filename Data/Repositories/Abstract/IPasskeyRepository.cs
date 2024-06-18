using DAL.Repositories.Abstract.Base;
using Domain.Passkey;

namespace DAL.Repositories.Abstract;

public interface IPasskeyRepository : IBaseRepository<PasskeyStoredCredential>
{
    Task<bool> IsCredentialIdOccupied(string credentialId, CancellationToken ct);
    Task<bool> DoesUserOwnCredential(int userId, string credentialId, CancellationToken ct);
}