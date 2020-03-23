using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyRates.Model;
using EasyRates.Writer;
using Microsoft.Extensions.Logging;

namespace EasyRates.RatesProvider.OpenExchange
{
    public class OpenExchangeRatesProviderForPoor:IRatesProvider
    {
        private readonly IOpenExchangeProxy proxy;
        private readonly ILogger<OpenExchangeRatesProviderForPoor> logger;

        private string[] currencies;
        
        public OpenExchangeRatesProviderForPoor(
            IOpenExchangeProxy proxy,
            IOpenExchangeRateProviderSettings settings,
            ILogger<OpenExchangeRatesProviderForPoor> logger)
        {
            this.proxy = proxy;
            this.logger = logger;
            Priority = settings.Priority;
            Name = settings.Name;
            currencies = settings.Currencies;
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
            var responses = new List<LatestRateResponse>();
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
                .Select(r => r.ToDomain(Name))
                .SelectMany(x => x)
                .ToArray();
        }
    }
}