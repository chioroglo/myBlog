namespace Common.Options;

public class PasskeyRelyingPartyOptions : BaseApplicationOptions
{
    public new static string Config => "PasskeyRelyingPartyOptions";

    public string Id { get; init; }
    public string Name { get; init; }
}