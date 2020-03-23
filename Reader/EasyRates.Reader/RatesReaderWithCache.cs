using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyRates.Model;
using Microsoft.Extensions.Caching.Memory;
using Time;

namespace EasyRates.Reader.Model
{
    public class RatesReaderWithCache : IRatesReader
    {
        private readonly IRatesReaderRepo repo;
        private readonly IMemoryCache cache;
        

        public RatesReaderWithCache(
            IRatesReaderRepo repo, 
            IMemoryCache cache)
        {
            this.repo = repo;
            this.cache = cache;
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
            return absoluteExpiration.ToAbsoluteExpirationRelativeNow(TimeSpan.FromSeconds(5));
        }
    }
}