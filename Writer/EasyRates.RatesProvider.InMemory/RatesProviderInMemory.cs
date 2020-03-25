using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyRates.Model;
using EasyRates.Writer;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Options;

namespace EasyRates.RatesProvider.InMemory
{
    public class RatesProviderInMemory:IRatesProvider
    {
        private readonly ISystemClock clock;
        private Random r = new Random();

        public RatesProviderInMemory(
            IOptions<RatesProviderInMemoryOptions> opts,
            ISystemClock clock)
        {
            this.clock = clock;
            Priority = opts.Value.Priority;
            Name = opts.Value.Name;
        }
        
        public int Priority { get; }
        
        public string Name { get; }
        
        public Task<CurrencyRate[]> GetAllRates()
        {
            var rub2usd = new decimal(r.Next(1, 100));
            var rub2eur = new decimal(r.Next(1, 100));

            var now = clock.UtcNow.UtcDateTime;
            var result = new[]
            {
                new CurrencyRate
                {
                    CurrencyFrom = "RUB",
                    CurrencyTo = "USD",
                    Value = rub2usd,
                    TimeOfReceipt = now,
                    OriginalPublishedTime = now,
                    ProviderName = Name
                },
                new CurrencyRate
                {
                    CurrencyFrom = "USD",
                    CurrencyTo = "RUB",
                    Value = 1 / rub2usd,
                    TimeOfReceipt = now,
                    OriginalPublishedTime = now,
                    ProviderName = Name
                },
                new CurrencyRate
                {
                    CurrencyFrom = "RUB",
                    CurrencyTo = "EUR",
                    Value = rub2eur,
                    TimeOfReceipt = now,
                    OriginalPublishedTime = now,
                    ProviderName = Name
                },
                new CurrencyRate
                {
                    CurrencyFrom = "EUR",
                    CurrencyTo = "RUB",
                    Value = 1 / rub2eur,
                    TimeOfReceipt = now,
                    OriginalPublishedTime = now,
                    ProviderName = Name
                }

            };

            return Task.FromResult(result);
        }
    }
}