using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Serilog;

namespace BlogHub.Api.Services;

public class LoggingDistributedCache : IDistributedCache
{
    private readonly RedisCache _cache;

    public LoggingDistributedCache(RedisCache cache) =>
        _cache = cache;

    public byte[]? Get(string key)
    {
        Log.Information($"Reading cache for key - {key}");
        return _cache.Get(key);
    }

    public async Task<byte[]?> GetAsync(string key, CancellationToken token = default)
    {
        Log.Information($"Reading cache for key - {key}");
        return await _cache.GetAsync(key, token);
    }

    public void Refresh(string key)
    {
        _cache.Refresh(key);
    }

    public async Task RefreshAsync(string key, CancellationToken token = default)
    {
        await _cache.RefreshAsync(key, token);
    }

    public void Remove(string key)
    {
        Log.Information($"Removing cache for key - {key}");
        _cache.Remove(key);
    }

    public async Task RemoveAsync(string key, CancellationToken token = default)
    {
        Log.Information($"Removing cache for key - {key}");
        await _cache.RemoveAsync(key, token);
    }

    public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
    {
        options = new DistributedCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromHours(1))
            .SetSlidingExpiration(TimeSpan.FromMinutes(10));

        Log.Information($"Setting cache for key - {key}");
        _cache.Set(key, value, options);
    }

    public async Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = default)
    {
        options = new DistributedCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromHours(1))
            .SetSlidingExpiration(TimeSpan.FromMinutes(10));

        Log.Information($"Setting cache for key - {key}");
        await _cache.SetAsync(key, value, options, token);
    }
}