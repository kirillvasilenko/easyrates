using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyRates.Model;
using EasyRates.Writer;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EasyRates.RatesProvider.OpenExchange
{
    public class OpenExchangeRatesProvider:IRatesProvider
    {
        private readonly IOpenExchangeProxy proxy;
        private readonly ISystemClock clock;
        private readonly ILogger<OpenExchangeRatesProvider> logger;

        private readonly string[] currencies;
        
        public OpenExchangeRatesProvider(
            IOpenExchangeProxy proxy,
            ISystemClock clock,
            IOptions<OpenExchangeProviderOptions> opts,
            ILogger<OpenExchangeRatesProvider> logger)
        {
            this.proxy = proxy;
            this.clock = clock;
            this.logger = logger;
            Priority = opts.Value.Priority;
            Name = opts.Value.Name;
            currencies = opts.Value.Currencies;
        }
        
        public int Priority { get; }
        
        public string Name { get; }
        
        public async Task<CurrencyRate[]> GetAllRates()
        {
            // run getting all the currencies
            var tasks = currencies
                .Select(currency => proxy.GetCurrentRates(currency))
                .ToList();

            // await and get responses, log if error
            var responses = new List<ActualRateResponse>();
            foreach (var task in tasks)
            {
                try
                {
                    responses.Add(await task);
                }
                catch (Exception e)
                {
                    logger.LogError(e, e.Message);
                }
            }

            return responses
                .Select(r => r.ToDomain(Name, clock.UtcNow.UtcDateTime))
                .SelectMany(x => x)
                .ToArray();
        }
    }
}