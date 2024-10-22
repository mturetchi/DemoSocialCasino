using DemoCasino.Services.Shared.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace DemoCasino.Services.Shared.Services;

class RedisCacheService : ICacheService
{
    private readonly IDatabase _redisDatabase;

    public RedisCacheService(IConnectionMultiplexer redis)
    {
        _redisDatabase = redis.GetDatabase();
    }

    public async Task<T> GetAsync<T>(string key)
    {
        var value = await _redisDatabase.StringGetAsync(key);
        if (!value.HasValue)
            return default;

        return JsonSerializer.Deserialize<T>(value);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        var serializedValue = JsonSerializer.Serialize(value);
        await _redisDatabase.StringSetAsync(key, serializedValue, expiry);
    }

    public Task RemoveAsync(string key) =>
        _redisDatabase.KeyDeleteAsync(key);
}