using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

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
                .AddSingleton<ITimetable>(c => new Timetable(anchorTime, updatePeriod))
                .AddSingleton<IRatesUpdater, RatesUpdater>()
                .AddSingleton<IWriterApp, WriterApp>()
                .AddTransient<ICurrencyRateProcessor, CurrencyRateProcessor>();
        }
    }
}