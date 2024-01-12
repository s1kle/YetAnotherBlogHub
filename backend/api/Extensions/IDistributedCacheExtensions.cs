using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace BlogHub.Api.Extensions;

public static class IDistributedCacheExtensions
{
    private static List<string> _keys { get; } = new ();

    public static async Task SetItemAsync<TItem>(this IDistributedCache cache, string key, TItem value, CancellationToken cancellationToken = default)
    {
        _keys.Add(key);
        var bytes = JsonSerializer.SerializeToUtf8Bytes(value);
        await cache.SetAsync(key, bytes, cancellationToken);
    }

    public static async Task<TItem?> GetItemAsync<TItem>(this IDistributedCache cache, string key, CancellationToken cancellationToken = default)
    {
        var bytes = await cache.GetAsync(key, cancellationToken);
        if (bytes is null) return default;
        var value = JsonSerializer.Deserialize<TItem>(bytes);
        return value;
    }
    public static async Task<TItem?> GetOrCreateItemAsync<TItem>(this IDistributedCache cache, string key, Func<Task<TItem?>> getItem, CancellationToken cancellationToken = default)
    {
        var value = await cache.GetItemAsync<TItem>(key, cancellationToken);
        if (value is null) 
        {
            value = await getItem();
            await cache.SetItemAsync(key, value, cancellationToken);
        }
        return value;
    }

    public static bool Contains(this IDistributedCache cache, string key) =>
        _keys.Contains(key);

    public static async Task ClearAsync(this IDistributedCache cache, CancellationToken cancellationToken)
    {
        foreach(var key in _keys) await cache.RemoveAsync(key, cancellationToken);
    }
}