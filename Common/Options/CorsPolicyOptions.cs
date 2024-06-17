namespace Common.Options;

public class CorsPolicyOptions : BaseApplicationOptions
{
    public new static string Config => "CorsPolicyOptions";
    public string[] AllowedOrigins { get; init; } = Array.Empty<string>();
    public string[] AllowedMethods { get; init; } = Array.Empty<string>();
}