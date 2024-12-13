using Core.Contract.Infrastructure;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System.Text.Json;

namespace Core.Infrastracture.Cashing;

public class RedisCacheService : IRedisCacheService
{
    private readonly IDistributedCache _cache;
    private readonly IConnectionMultiplexer _redisConnection;
    private readonly string _prefix = string.Empty;

    public RedisCacheService(IDistributedCache cache, IConnectionMultiplexer redisConnection, string prefix)
    {
        _cache = cache;
        _redisConnection = redisConnection;
        _prefix = prefix;
    }


    public async Task<T> Get<T>(string key)
    {
        var prefixedKey = CreateKey<T>(key);
        var jsonData = await _cache.GetStringAsync(prefixedKey);
        return jsonData == null ? default : JsonSerializer.Deserialize<T>(jsonData);
    }

    public async Task<IEnumerable<T>> GetAll<T>()
    {
        var result = new List<T>();
        //var database = _redisConnection.GetDatabase();
        var server = _redisConnection.GetServer(_redisConnection.GetEndPoints().FirstOrDefault());
        var typePrefix = CreateTypePrefix<T>();
        var keys = server.Keys(pattern: $"{_prefix}_{typePrefix}*").ToList();

        foreach (var key in keys)
        {
            var value = await _cache.GetStringAsync(key);
            if (value != null)
                try
                {
                    var item = JsonSerializer.Deserialize<T>(value);
                    result.Add(item);
                }
                catch
                {
                    continue;
                }
        }
        return result;
    }

    public async Task Remove<T>(string key)
    {
        var prefixedKey = CreateKey<T>(key);
        await _cache.RemoveAsync(prefixedKey);
    }

    public async Task Set<T>(string key, T value, TimeSpan? expiry = null)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiry ?? TimeSpan.FromHours(1),
            SlidingExpiration = TimeSpan.FromMinutes(1)
        };
        var prefixedKey = CreateKey<T>(key);
        var jsonData = JsonSerializer.Serialize(value);
        await _cache.SetStringAsync(prefixedKey, jsonData, options);
    }

    private string CreateKey<T>(string key) => string.IsNullOrEmpty(_prefix) ? $"{CreateTypePrefix<T>()}:{key}" : $"{_prefix}_{CreateTypePrefix<T>()}:{key}";
    private string CreateTypePrefix<T>() => typeof(T).Name.ToLower();
}
