namespace Common.Options;

public class CacheOptions : BaseApplicationOptions
{
    public new static string Config => "CacheOptions"; 
    public int DefaultExpirationInMinutes { get; init; }
}