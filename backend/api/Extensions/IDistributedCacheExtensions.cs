using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace BlogHub.Api.Extensions
{
    public static class IDistributedCacheExtensions
    {
        public static async Task SetItemAsync<TItem>(this IDistributedCache cache, string key, TItem value, CancellationToken cancellationToken = default)
        {
            var prefix = GetPrefix(key);
            var actualKey = RemovePrefix(key);
            var currentKey = await cache.GetStringAsync(prefix);
            await cache.SetStringAsync(prefix, $"{actualKey}{currentKey ?? ""}");
            var bytes = JsonSerializer.SerializeToUtf8Bytes(value);
            await cache.SetAsync(actualKey, bytes, cancellationToken);
        }

        public static async Task<TItem?> GetItemAsync<TItem>(this IDistributedCache cache, string key, CancellationToken cancellationToken = default)
        {
            var actualKey = RemovePrefix(key);
            var bytes = await cache.GetAsync(actualKey, cancellationToken);
            if (bytes is null) return default;
            var value = JsonSerializer.Deserialize<TItem>(bytes);
            return value;
        }

        public static async Task<TItem?> GetOrCreateItemAsync<TItem>(this IDistributedCache cache, string key, Func<Task<TItem?>> getItem, CancellationToken cancellationToken = default)
        {
            var actualKey = RemovePrefix(key);
            var value = await cache.GetItemAsync<TItem>(actualKey, cancellationToken);
            if (value is null)
            {
                value = await getItem();
                await cache.SetItemAsync(key, value, cancellationToken);
            }
            return value;
        }

        public static async Task<bool> ContainsAsync(this IDistributedCache cache, string key, CancellationToken cancellationToken)
        {
            var prefix = GetPrefix(key);
            var actualKey = RemovePrefix(key);
            var currentKey = await cache.GetStringAsync(prefix, cancellationToken);
            return currentKey?.Contains(actualKey) ?? false;
        }

        public static async Task ClearAsync(this IDistributedCache cache, string prefix, CancellationToken cancellationToken) =>
            await cache.RemoveAsync(prefix, cancellationToken);

        private static string RemovePrefix(string key) => new string(
            key.Skip(key.IndexOf(':') + 1).ToArray());

        private static string GetPrefix(string key) => new string(
            key.Take(key.IndexOf(':')).ToArray());
    }
}
