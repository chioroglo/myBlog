using Domain.Abstract;

namespace Domain;

public class Passkey : BaseEntity
{
    public User User { get; set; }
    public int UserId { get; set; }
    public string CredentialId { get; set; }
    public string PublicKey { get; set; }
    public string CredentialType { get; set; }
    public bool IsActive { get; set; }
    public string AaGuid { get; set; }
}