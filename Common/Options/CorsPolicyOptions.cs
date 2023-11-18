namespace Common.Options;

public class CorsPolicyOptions : ApplicationOptions
{
    public static string Config => "CorsPolicyOptions";
    public string[] AllowedOrigins { get; init; } = Array.Empty<string>();
    public string[] AllowedMethods { get; init; } = Array.Empty<string>();
}