using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EasyRates.Model;
using EasyRates.Writer;
using Microsoft.Extensions.Logging;

namespace EasyRates.WriterApp
{
    public class RatesUpdater : IRatesUpdater
    {
        private readonly IRatesWriter writer;
        
        private readonly ICurrencyRateProcessor processor;

        private readonly ICollection<IRatesProvider> ratesProviders;

        private readonly ILogger<RatesUpdater> logger;

        public RatesUpdater(
            IRatesWriter writer,
            IOrderedRatesProviders ratesProviders,
            ICurrencyRateProcessor processor,
            ILogger<RatesUpdater> logger)
        {
            this.writer = writer;
            this.processor = processor;
            this.ratesProviders = ratesProviders.GetProviders();
            this.logger = logger;
        }
        
        public async Task UpdateRates(CancellationToken ct)
        {
            var actualRates = new Dictionary<string, CurrencyRate>();
            var allGotRates = new List<CurrencyRate>();

            ct.ThrowIfCancellationRequested();
                
            // start getting rates from all providers simultaneously
            var tasks = ratesProviders.Select(p =>
            {
                logger.LogDebug($"Ask {p.Name} for all rates.");
                var task = p.GetAllRates();
                return (task, p.Name);
            }).ToList();

            // get result from the least significant provider
            foreach (var (task, name) in tasks)
            {
                try
                {
                    var gotRates = await task;
                    logger.LogDebug($"Got {gotRates.Length} rates from {name}.");
                
                    processor.Process(gotRates);
                
                    allGotRates.AddRange(gotRates);
                    
                    // As a result we have all available rates
                    // from the most priority providers.
                    foreach (var rate in gotRates)
                    {
                        actualRates[rate.Key] = rate;
                    }
                }
                catch (Exception e)
                {
                    logger.LogError(e, e.Message);
                }
            }
            
            await writer.AddRatesToHistory(allGotRates);
            await writer.SetActualRates(actualRates.Values);
            ct.ThrowIfCancellationRequested();
            await writer.SaveChanges();
        }

        
    }
}