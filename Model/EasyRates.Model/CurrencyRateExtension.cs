using System;
using AutoMapper;

namespace EasyRates.Model
{
    public static class CurrencyRateExtension
    {
        public static void Update(this CurrencyRate destination, CurrencyRate source, IMapper mapper)
        {
            mapper.Map(source, destination);
        }
        
        public static CurrencyRateHistoryItem ToHistoryItem(this CurrencyRate source, IMapper mapper)
        {
            return mapper.Map<CurrencyRateHistoryItem>(source);
        }
    }
}