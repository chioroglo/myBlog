namespace Common.Options;

public class CorsPolicyOptions : IApplicationOptions
{
    public static string Config => "CorsPolicyOptions";
    public string[] AllowedOrigins { get; init; } = Array.Empty<string>();
    public string[] AllowedMethods { get; init; } = Array.Empty<string>();
}