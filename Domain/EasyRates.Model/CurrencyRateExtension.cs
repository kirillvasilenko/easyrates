using System;
using AutoMapper;

namespace EasyRates.Model
{
    public static class CurrencyRateExtension
    {
        
        static CurrencyRateExtension()
        {
            var config = new MapperConfiguration(cnfg =>
            {
                cnfg.CreateMap<CurrencyRate, CurrencyRate>();
                cnfg.CreateMap<CurrencyRate, CurrencyRateHistoryItem>();
            });
            Mapper = config.CreateMapper();
        }

        private static readonly IMapper Mapper;
        
        public static void Update(this CurrencyRate destination, CurrencyRate source)
        {
            Mapper.Map(source, destination);
        }
        
        public static CurrencyRateHistoryItem ToHistoryItem(this CurrencyRate source)
        {
            return Mapper.Map<CurrencyRateHistoryItem>(source);
        }
    }
}