using System.Collections.Generic;
using System.Linq;
using EasyRates.Model;
using Time;

namespace EasyRates.RatesProvider.OpenExchange
{
    public static class LatestRateResponseConverter
    {
        public static ICollection<CurrencyRate> ToDomain(this LatestRateResponse response, string providerName)
        {
            return response.Rates.Select(kv => 
                new CurrencyRate
                {
                    From = response.Base,
                    ProviderName = providerName,
                    To = kv.Key,
                    Value = kv.Value,
                    TimeOfReceipt = TimeProvider.UtcNow,
                    OriginalPublishedTime = response.TimeStampAsTime
                }).ToList();
        }
    }
}