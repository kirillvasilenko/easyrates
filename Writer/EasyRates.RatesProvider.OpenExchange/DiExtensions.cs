using System;
using EasyRates.Writer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EasyRates.RatesProvider.OpenExchange
{
    public static class DiExtensions
    {
        public static IServiceCollection AddEasyRatesProviderOpenExchange(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var appId = configuration["AppId"];

            services.AddHealthChecks()
                .AddCheck<OpenExchangeHealthCheck>("openExchangeHealthCheck");
            
            return services
                .Configure<OpenExchangeProviderOptions>(configuration)
                .AddTransient<IOpenExchangeProxy>(c => new OpenExchangeProxy(appId))
                .AddTransient<IRatesProvider, OpenExchangeRatesProvider>();
        }
        
    }
}