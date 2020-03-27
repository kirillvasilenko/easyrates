using EasyRates.Model.Ef.Pg;
using EasyRates.Reader.Model;
using Microsoft.Extensions.DependencyInjection;

namespace EasyRates.Reader.Ef.Pg
{
    public static class DiExtensions
    {
        public static IServiceCollection AddEasyRatesReaderEfPg(this IServiceCollection services, string connectionString)
        {
            return services
                .AddEasyRatesModelEfPg(connectionString)
                .AddEasyRatesReader()
                .AddScoped<IRatesReaderRepo, RatesReaderRepoEf>();
        }
    }
}