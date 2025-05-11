using System.ComponentModel.DataAnnotations;

namespace MyBlog.Common.Options;

public class CacheOptions : BaseApplicationOptions
{
    public new static string Config => "CacheOptions";
    [Required]
    public TimeSpan DefaultExpiration { get; set; }
    public CacheProvider Provider { get; init; } = CacheProvider.InMemory;
}

public enum CacheProvider
{
    InMemory = 0,
    Redis = 1
}
