using EasyRates.Model.Ef.Pg;
using Microsoft.Extensions.DependencyInjection;

namespace EasyRates.Writer.Ef.Pg
{
    public static class DiExtensions
    {
        public static IServiceCollection AddEasyRatesWriterEfPg(
            this IServiceCollection services,
            string connectionString,
            ServiceLifetime dbContextLifetime = ServiceLifetime.Scoped)
        {
            return services
                .AddEasyRatesModelEfPg(connectionString, dbContextLifetime)
                .AddEasyRatesWriter()
                .AddTransient<IRatesWriter, RatesWriterEf>();
        }
    }
}