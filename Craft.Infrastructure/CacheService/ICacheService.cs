namespace Craft.Infrastructure.CacheService;

public interface ICacheService
{
    void Remove(string cacheKey);

    T Set<T>(string cacheKey, T value);

    (bool, T) TryGet<T>(string cacheKey);
}
