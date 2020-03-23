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
            
            return services
                .Configure<OpenExchangeProviderOptions>(configuration)
                .AddScoped<IOpenExchangeProxy>(c => new OpenExchangeProxy(appId))
                .AddScoped<IRatesProvider, OpenExchangeRatesProvider>();
        }
        
    }
}