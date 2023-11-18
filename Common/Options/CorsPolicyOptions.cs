namespace Common.Options;

public class CorsPolicyOptions : IApplicationOptions
{
    public static string Config => "CorsPolicyOptions";
    public string[] AllowedOrigins { get; init; } = new string[] { };
    public string[] AllowedMethods { get; init; } = new string[] { };
}