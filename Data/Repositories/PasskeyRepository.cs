using DAL.Repositories.Abstract;
using DAL.Repositories.Abstract.Base;
using Domain.Passkey;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class PasskeyRepository(BlogDbContext db) : BaseRepository<PasskeyStoredCredential>(db), IPasskeyRepository
{
    public async Task<bool> IsCredentialIdOccupied(string credentialId, CancellationToken ct)
    {
        return await _db.PasskeyStoredCredentials.AnyAsync(p => p.CredentialId == credentialId, ct);
    }

    public async Task<bool> DoesUserOwnCredential(int userId, string credentialId, CancellationToken ct)
    {
        return await _db.PasskeyStoredCredentials.AnyAsync(p => p.UserId == userId && p.CredentialId == credentialId, ct);
    }
}