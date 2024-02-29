namespace Craft.Infrastructure.CacheService;

public class RedisCacheService : ICacheService
{
    public void Remove(string cacheKey)
    {
        throw new NotImplementedException();
    }

    public T Set<T>(string cacheKey, T value)
    {
        throw new NotImplementedException();
    }

    public (bool, T) TryGet<T>(string cacheKey)
    {
        throw new NotImplementedException();
    }
}
