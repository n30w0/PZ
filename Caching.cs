using System;
using Microsoft.Extensions.Caching.Memory;

namespace Client_WPF.Cache
{
    public class Caching<T>
    {
        private MemoryCache _cache = new MemoryCache(new MemoryCacheOptions()
        {
            SizeLimit = 1024
        });

        public T GetOrCreate(object key, T Tvalue)
        {
            T cacheEntry;
            if (!_cache.TryGetValue(key, out cacheEntry))
            {
                cacheEntry = Tvalue;

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSize(1)
                    .SetPriority(CacheItemPriority.High)
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(15));

                _cache.Set(key, cacheEntry, cacheEntryOptions);
            }
            return cacheEntry;
        }

        public void Remove(object key)
        {
            _cache.Remove(key);
        }

    }
}