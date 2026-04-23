using Application.Abstractions.Caching;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Caching
{
    public class MemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _cache;

        public MemoryCacheService(IMemoryCache cache) { _cache = cache; }

        public Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            _cache.TryGetValue(key, out T? value);
            return Task.FromResult(value);
        }

        public Task SetAsync<T>(
            string key,
            T value, 
            TimeSpan? expireation = null,
            CancellationToken cancellationToken = default)
        {
            var options = new MemoryCacheEntryOptions();

            if (expireation.HasValue)
                options.SetSlidingExpiration(expireation.Value);

            _cache.Set(key, value, options);

            return Task.CompletedTask;
        }

        public Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            _cache.Remove(key);
            return Task.CompletedTask;
        }
    }
}
