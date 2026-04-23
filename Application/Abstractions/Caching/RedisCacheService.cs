using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Application.Abstractions.Caching
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _cache;

        public RedisCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            var value = await _cache.GetStringAsync(key, cancellationToken);
            if (value is null)
                return default;

            return JsonSerializer.Deserialize<T>(value);
        }

        public async Task SetAsync<T>(
            string key, 
            T value, 
            TimeSpan? expiration = null,
            CancellationToken cancellationToken = default)
        {
            var json = JsonSerializer.Serialize(value);

            var options = new DistributedCacheEntryOptions();

            if (expiration.HasValue)
                options.SetSlidingExpiration(expiration.Value);

            await _cache.SetStringAsync(key, json, options, cancellationToken);
        }

        public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            await _cache.RemoveAsync(key, cancellationToken);
        }
    }
}
