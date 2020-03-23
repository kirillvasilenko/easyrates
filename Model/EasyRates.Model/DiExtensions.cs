using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace EasyRates.Model
{
    public static class DiExtensions
    {
        public static IServiceCollection AddEasyRatesModel(this IServiceCollection services)
        {
            return services
                .AddAutoMapper(typeof(DiExtensions).Assembly)
                .AddSingleton<ICurrencyNameFormatter, CurrencyNameFormatter>()
                .AddSingleton<ICurrencyNameValidator, CurrencyNameValidator>();
        }
    }
}