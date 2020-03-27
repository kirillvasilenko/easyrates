using Microsoft.Extensions.DependencyInjection;

namespace EasyRates.Writer
{
    public static class DiExtensions
    {
        public static IServiceCollection AddEasyRatesWriter(this IServiceCollection services)
        {
            return services.AddTransient<IOrderedRatesProviders, OrderedRatesProviders>();
        }
    }
}