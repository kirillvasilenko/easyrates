using Microsoft.Extensions.DependencyInjection;

namespace EasyRates.ReaderApp
{
    public static class DiExtensions
    {
        public static IServiceCollection AddEasyRatesReaderApp(this IServiceCollection services)
        {
            return services
                .AddScoped<IReaderRateService, ReaderRateService>();
        }
    }
}