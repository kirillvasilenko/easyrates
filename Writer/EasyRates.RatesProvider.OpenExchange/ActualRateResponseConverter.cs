using System;
using System.Collections.Generic;
using System.Linq;
using EasyRates.Model;

namespace EasyRates.RatesProvider.OpenExchange
{
    public static class ActualRateResponseConverter
    {
        public static ICollection<CurrencyRate> ToDomain(
            this ActualRateResponse response, 
            string providerName,
            DateTime currentTimeUtc)
        {
            return response.Rates.Select(kv => 
                new CurrencyRate
                {
                    From = response.Base,
                    ProviderName = providerName,
                    To = kv.Key,
                    Value = kv.Value,
                    TimeOfReceipt = currentTimeUtc,
                    OriginalPublishedTime = response.TimeStamp.ToDateTimeUtc()
                }).ToList();
        }
        
        public static DateTime ToDateTimeUtc(this long timestampUnix)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(timestampUnix);
        } 
    }
}