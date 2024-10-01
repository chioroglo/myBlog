using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Common.Dto.Auth;

public class AuthenticatePasskeyRequest
{
    [Required]
    public string CredentialId { get; init; }
    [Required]
    public string AuthenticatorData { get; init; }
    [Required]
    public string Signature { get; init; }
    [Required]
    public string Type { get; init; }
    [Required]
    public string Challenge { get; init; }
    [Required]
    public string ClientDataJson { get; init; }
    [Required]
    public string UserHandle { get; init; }

    [NotMapped]
    public int? UserId
    {
        get
        {
            if (string.IsNullOrWhiteSpace(UserHandle))
            {
                return null;
            }

            var base64EncodedBytes = Convert.FromBase64String(UserHandle);
            var utf8String = Encoding.UTF8.GetString(base64EncodedBytes);

            var parsedSuccessfully = int.TryParse(utf8String, out var userId);
            return parsedSuccessfully ? userId : null;
        }
    }
}