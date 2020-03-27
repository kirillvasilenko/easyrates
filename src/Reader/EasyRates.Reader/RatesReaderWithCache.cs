using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyRates.Model;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Internal;

namespace EasyRates.Reader.Model
{
    public class RatesReaderWithCache : IRatesReader
    {
        private readonly IRatesReaderRepo repo;
        private readonly IMemoryCache cache;
        private readonly ISystemClock clock;


        public RatesReaderWithCache(
            IRatesReaderRepo repo, 
            IMemoryCache cache,
            ISystemClock clock)
        {
            this.repo = repo;
            this.cache = cache;
            this.clock = clock;
        }
        
        public async Task<CurrencyRate> GetRate(string @from, string to)
        {
            var key = from + to;

            if (cache.TryGetValue(key, out CurrencyRate result))
            {
                return result;
            }
            
            result = await repo.GetRate(@from, to);
            cache.Set(key, result, 
                GetAbsoluteExpirationRelativeNow(result.ExpirationTime));
            return result;
        }

        public async Task<ICollection<CurrencyRate>> GetRates(string @from)
        {
            if (cache.TryGetValue(from, out CurrencyRate[] result))
            {
                return result;
            }
            
            result = await repo.GetRates(@from);
            cache.Set(from, result,
                GetAbsoluteExpirationRelativeNow(result.First().ExpirationTime));
            return result;
        }
        
        private TimeSpan GetAbsoluteExpirationRelativeNow(DateTime absoluteExpiration)
        {
            var now = clock.UtcNow;
            return absoluteExpiration > now 
                ? absoluteExpiration - now
                : TimeSpan.FromSeconds(5);
        }
    }
}