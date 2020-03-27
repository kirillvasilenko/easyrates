using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace EasyRates.RatesProvider.OpenExchange
{
    public class OpenExchangeHealthCheck : IHealthCheck
    {
        private readonly IOpenExchangeProxy openExchangeProxy;
        private readonly ILogger<OpenExchangeHealthCheck> logger;

        public OpenExchangeHealthCheck(
            IOpenExchangeProxy openExchangeProxy,
            ILogger<OpenExchangeHealthCheck> logger)
        {
            this.openExchangeProxy = openExchangeProxy;
            this.logger = logger;
        }
        
        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                await openExchangeProxy.GetUsage();
                return HealthCheckResult.Healthy();
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return HealthCheckResult.Unhealthy(e.Message);
            }
        }
    }
}