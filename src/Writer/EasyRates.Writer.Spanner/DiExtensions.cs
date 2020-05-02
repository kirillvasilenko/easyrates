using EasyRates.Model.Spanner;
using Microsoft.Extensions.DependencyInjection;

namespace EasyRates.Writer.Spanner
{
    public static class DiExtensions
    {
        public static IServiceCollection AddEasyRatesWriterSpanner(
            this IServiceCollection services,
            string connectionString,
            ServiceLifetime dbContextLifetime = ServiceLifetime.Scoped)
        {
            return services
                .AddEasyRatesModelSpanner(connectionString, dbContextLifetime)
                .AddEasyRatesWriter()
                .AddTransient<IRatesWriter, RatesWriterSpanner>();
        }
    }
}