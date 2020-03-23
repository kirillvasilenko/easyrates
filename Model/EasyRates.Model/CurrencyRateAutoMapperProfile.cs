using AutoMapper;

namespace EasyRates.Model
{
    public class CurrencyRateAutoMapperProfile : Profile
    {
        public CurrencyRateAutoMapperProfile()
        {
            CreateMap<CurrencyRate, CurrencyRate>();
            CreateMap<CurrencyRate, CurrencyRateHistoryItem>();
        }
    }
}