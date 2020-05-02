using EasyRates.Model.Spanner;
using EasyRates.Reader.Model;
using Microsoft.Extensions.DependencyInjection;

namespace EasyRates.Reader.Spanner
{
    public static class DiExtensions
    {
        public static IServiceCollection AddEasyRatesReaderEfSpanner(this IServiceCollection services, string connectionString)
        {
            return services
                .AddEasyRatesModelSpanner(connectionString)
                .AddEasyRatesReader()
                .AddScoped<IRatesReaderRepo, RatesReaderRepoEfSpanner>();
        }
    }
}