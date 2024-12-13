namespace Core.Contract.Infrastructure;

public interface IRedisCacheService
{
    public Task Set<T>(string key, T value, TimeSpan? expiry = null);
    public Task<T> Get<T>(string key);
    public Task Remove<T>(string key);
    public Task<IEnumerable<T>> GetAll<T>();
}
