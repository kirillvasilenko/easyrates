using System;
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
                    From = "RUB",
                    To = "USD",
                    Value = rub2usd,
                    TimeOfReceipt = now,
                    OriginalPublishedTime = now,
                    ProviderName = Name
                },
                new CurrencyRate
                {
                    From = "USD",
                    To = "RUB",
                    Value = 1 / rub2usd,
                    TimeOfReceipt = now,
                    OriginalPublishedTime = now,
                    ProviderName = Name
                },
                new CurrencyRate
                {
                    From = "RUB",
                    To = "EUR",
                    Value = rub2eur,
                    TimeOfReceipt = now,
                    OriginalPublishedTime = now,
                    ProviderName = Name
                },
                new CurrencyRate
                {
                    From = "EUR",
                    To = "RUB",
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