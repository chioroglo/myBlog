namespace Common.Dto.Auth;

public record PasskeyUserEntityModel
{
    public string Id { get; set; }
    public string DisplayName { get; set; }
    public string Name { get; set; }
}