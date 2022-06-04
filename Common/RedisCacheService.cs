using StackExchange.Redis;

namespace StackApi.Common;

public class RedisCacheService : ICacheService
{
    public readonly IConnectionMultiplexer connectionMultiplexer;

    public RedisCacheService(IConnectionMultiplexer _multiplexer)
    {
        connectionMultiplexer = _multiplexer;
    }
    public async Task<string> GetCacheValueAsync(string key)
    {
        var db = connectionMultiplexer.GetDatabase();
        var IsExpired = await db.ExecuteAsync("ttl ", new object[] { key });
        if ((int)IsExpired >= 0)
        {
            return await db.StringGetAsync(key);
        }
        else
        {
            return null;
        }
    }

    public async Task SetCacheValueAsync(string key, string value)
    {
        var db = connectionMultiplexer.GetDatabase();
        await db.StringSetAsync(key, value, new TimeSpan(0, 5, 0));
    }
}