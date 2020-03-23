using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EasyRates.WriterApp
{
    public static class DiExtensions
    {
        public static IServiceCollection AddEasyRatesWriterApp(
            this IServiceCollection services,
            IConfiguration config)
        {
            var anchorTime = config.GetValue("AnchorTime", TimeSpan.FromMinutes(1));
            var updatePeriod = config.GetValue("UpdatePeriod", TimeSpan.FromHours(1));
            
            return services
                .AddScoped<ITimetable>(c => new Timetable(anchorTime, updatePeriod))
                .AddScoped<ICurrencyRateProcessor, CurrencyRateProcessor>()
                .AddScoped<IRatesUpdater, RatesUpdater>()
                .AddScoped<IWriterApp, WriterApp>();
        }
    }
}