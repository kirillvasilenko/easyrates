using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace EasyRates.Reader.Model
{
    public static class DiExtensions
    {
        public static IServiceCollection AddEasyRatesReader(this IServiceCollection services)
        {
            return services
                .AddMemoryCache(opts =>
                {
                    // takes from DI
                    // opts.Clock = ...
                })
                .AddScoped<IRatesReader, RatesReaderWithCache>();
        }
    }
}