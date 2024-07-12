namespace Service.Abstract;

public interface ICacheService
{
    Task SetAsync<T>(string cacheKey, T value, TimeSpan? expiration = null);
    Task<T?> GetAsync<T>(string cacheKey);
    Task<string?> GetStringAsync(string cacheKey);
    Task RemoveAsync(string cacheKey);
}