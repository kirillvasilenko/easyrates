using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EasyRates.Model.Ef.Pg
{
    public static class DiExtensions
    {
        public static IServiceCollection AddEasyRatesModelEfPg(this IServiceCollection services, string connectionString)
        {
            return services
                .AddEasyRatesModel()
                .AddDbContext<RatesContext>(options =>
                    options
                        .UseNpgsql(connectionString)
                        .UseSnakeCaseNamingConvention());

        }
    }
}