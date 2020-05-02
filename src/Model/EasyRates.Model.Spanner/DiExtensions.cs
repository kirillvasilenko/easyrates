using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace EasyRates.Model.Spanner
{
    public static class DiExtensions
    {
        public static IServiceCollection AddEasyRatesModelSpanner(
            this IServiceCollection services, 
            string connectionString,
            ServiceLifetime repoLifetime = ServiceLifetime.Scoped)
        {
            services.AddEasyRatesModel();
            if (repoLifetime == ServiceLifetime.Singleton)
            {
                services.AddSingleton(c =>
                {
                    var mapper = c.GetService<IMapper>();
                    return new RatesRepoSpanner(connectionString, mapper);
                });
            }
            else
            {
                services.AddScoped(c =>
                {
                    var mapper = c.GetService<IMapper>();
                    return new RatesRepoSpanner(connectionString, mapper);
                });
            }

            return services;
        }
        
        
    }
}