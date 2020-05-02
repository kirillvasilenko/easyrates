using EasyRates.RatesProvider.InMemory;
using EasyRates.RatesProvider.OpenExchange;
using EasyRates.Writer.Ef.Pg;
using EasyRates.Writer.Spanner;
using EasyRates.WriterApp.AspNet.HostedServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Internal;

namespace EasyRates.WriterApp.AspNet
{
    public static class DiExtensions
    {
        public static IServiceCollection AddEasyRatesWriterAppAspNet(
            this IServiceCollection services,
            IConfiguration config)
        {
            services
                .AddEasyRatesWriterApp(config.GetSection("Timetable"))
                .AddHostedService<WriterHostedService>()
                .AddSingleton<ISystemClock, SystemClock>();
            
            if (config.GetValue<string>("DbType") == "Spanner")
            {
                services.AddEasyRatesWriterSpanner(config.GetConnectionString("Spanner"),
                    ServiceLifetime.Singleton);
            }
            else
            {
                services.AddEasyRatesWriterEfPg(config.GetConnectionString("DefaultConnection"),
                    ServiceLifetime.Singleton);
            }
            
            var inMemoryProviderConfig = config.GetSection("ProviderInMemory");
            if (inMemoryProviderConfig.GetValue("Enabled", false))
            {
                services.AddEasyRatesProviderInMemory(inMemoryProviderConfig);
            }
            
            var openExchangeConfig = config.GetSection("ProviderOpenExchange");
            if (openExchangeConfig.GetValue("Enabled", false))
            {
                services.AddEasyRatesProviderOpenExchange(openExchangeConfig);
            }
            
            return services;
        }
    }
}