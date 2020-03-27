using System.Collections.Generic;
using System.Linq;
using EasyRates.Model;

namespace EasyRates.ReaderApp.Dto
{
    public static class CurrencyRateToDtoConverter
    {
        
        public static RatesResponse ToDto(this CurrencyRate rate)
        {
            return new[] {rate}.ToDto();
        }
        
        public static RatesResponse ToDto(this ICollection<CurrencyRate> rates)
        {
            return new RatesResponse
            {
                CurrencyFrom = rates.First().CurrencyFrom,
                Rates = rates.Select(ToRateInfo).ToArray()
            };
        }

        public static RateInfo ToRateInfo(this CurrencyRate rate)
        {
            return new RateInfo
            {
                CurrencyTo = rate.CurrencyTo,
                Rate = rate.Value,
                ExpireAt = rate.ExpirationTime
            };
        }
    }
}