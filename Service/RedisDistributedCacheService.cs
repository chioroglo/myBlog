using System.Text.Json;
using Common.Options;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Service.Abstract;

namespace Service;



public class RedisDistributedCacheService(IDistributedCache distributedCache, IOptions<CacheOptions> options) : ICacheService
{
    private readonly DistributedCacheEntryOptions _cacheOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(options.Value.DefaultExpirationInMinutes)
    };

    public async Task<T?> GetAsync<T>(string cacheKey)
    {
        var stringValue = await distributedCache.GetStringAsync(cacheKey);
        return string.IsNullOrWhiteSpace(stringValue) ? default! : JsonSerializer.Deserialize<T>(stringValue);
    }

    public async Task<string> GetStringAsync(string cacheKey)
    {
        var stringValue = await distributedCache.GetStringAsync(cacheKey);

        return string.IsNullOrWhiteSpace(stringValue) ? default! : stringValue;
    }

    public async Task RemoveAsync(string cacheKey)
    {
        await distributedCache.RemoveAsync(cacheKey);
    }

    public async Task SetAsync<T>(string cacheKey, T value, TimeSpan? expiration = null)
    {
        if (value is null)
            return;

        var serializedValue = JsonSerializer.Serialize(value);

        if (expiration is not null)
        {
            await distributedCache.SetStringAsync(cacheKey, serializedValue, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = expiration });
            return;
        }

        await distributedCache.SetStringAsync(cacheKey, serializedValue, _cacheOptions);
    }
}