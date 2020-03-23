using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EasyRates.Model.Ef.Pg
{
    public static class DiExtensions
    {
        public static IServiceCollection AddSocialNetworkRepoMySql(this IServiceCollection services, string connectionString)
        {
            return services
                .AddDbContext<RatesContext>(options =>
                    options
                        .UseNpgsql(connectionString)
                        .UseSnakeCaseNamingConvention());

        }
    }
}