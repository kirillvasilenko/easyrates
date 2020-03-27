using System.Linq.Expressions;
using AutoMapper;

namespace EasyRates.Model
{
    public class CurrencyRateAutoMapperProfile : Profile
    {
        public CurrencyRateAutoMapperProfile()
        {
            CreateMap<CurrencyRate, CurrencyRate>();

            CreateMap<CurrencyRate, CurrencyRateHistoryItem>()
                .ForMember(d => d.Id, x => x.Ignore());
        }
    }
}