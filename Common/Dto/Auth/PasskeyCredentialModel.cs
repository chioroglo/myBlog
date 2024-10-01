namespace Common.Dto.Auth;

public record PasskeyCredentialModel(string Id)
{
    public string Id { get; set; } = Id;
    public string Type { get; set; } = "public-key";
}