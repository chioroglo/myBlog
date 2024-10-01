namespace Common.Dto.Auth;

public record PasskeyRelyingPartyModel
{
    // Domain name of a website
    public string Id { get; init; }
    public string Name { get; init; }
    public string? Icon { get; init; }
}