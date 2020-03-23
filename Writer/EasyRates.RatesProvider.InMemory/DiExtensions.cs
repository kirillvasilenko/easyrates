using EasyRates.Writer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace EasyRates.RatesProvider.InMemory
{
    public static class DiExtensions
    {
        public static IServiceCollection AddEasyRatesProviderInMemory(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return services
                .Configure<RatesProviderInMemoryOptions>(configuration)
                .AddScoped<IRatesProvider, RatesProviderInMemory>();
        }
    }
}