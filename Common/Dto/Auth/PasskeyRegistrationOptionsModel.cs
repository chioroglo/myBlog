namespace Common.Dto.Auth;

public record PasskeyRegistrationOptionsModel
{
    public string Challenge { get; init; }
    public PasskeyRelyingPartyModel RelyingParty { get; init; }
    public PasskeyUserEntityModel User { get; init; }
    public IEnumerable<PasskeyCredentialModel> ExcludeCredentials { get; set; }
}