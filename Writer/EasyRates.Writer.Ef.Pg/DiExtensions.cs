using EasyRates.Model.Ef.Pg;
using Microsoft.Extensions.DependencyInjection;

namespace EasyRates.Writer.Ef.Pg
{
    public static class DiExtensions
    {
        public static IServiceCollection AddEasyRatesWriterEfPg(this IServiceCollection services, string connectionString)
        {
            return services
                .AddEasyRatesModelEfPg(connectionString)
                .AddScoped<IRatesWriter, RatesWriterEf>();
        }
    }
}