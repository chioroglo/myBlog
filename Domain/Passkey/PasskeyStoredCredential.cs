using Domain.Abstract;

namespace Domain.Passkey;

public class PasskeyStoredCredential : BaseEntity
{
    public User User { get; set; }
    public int UserId { get; set; }
    public string CredentialId { get; set; }
    public string PublicKey { get; set; }
    public string UserHandle { get; set; }
}