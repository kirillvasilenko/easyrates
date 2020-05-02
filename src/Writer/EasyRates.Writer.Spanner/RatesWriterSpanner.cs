using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EasyRates.Model;
using EasyRates.Model.Spanner;

namespace EasyRates.Writer.Spanner
{
    public class RatesWriterSpanner:IRatesWriter
    {
        private readonly RatesRepoSpanner repo;
        private readonly IMapper mapper;

        public RatesWriterSpanner(RatesRepoSpanner repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }
        
        public async Task SetActualRates(ICollection<CurrencyRate> rates)
        {
            await repo.SetActualRates(rates);
        }

        public async Task AddRatesToHistory(ICollection<CurrencyRate> rates)
        {
            var historyItems = rates.Select(r => r.ToHistoryItem(mapper)).ToList();
            await repo.AddRatesToHistory(historyItems);
        }

        public Task SaveChanges()
        {
            return Task.CompletedTask;
        }
    }
}