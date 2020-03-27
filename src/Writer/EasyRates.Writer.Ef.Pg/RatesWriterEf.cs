using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EasyRates.Model;
using EasyRates.Model.Ef.Pg;
using Microsoft.EntityFrameworkCore;

namespace EasyRates.Writer.Ef.Pg
{
    public class RatesWriterEf:IRatesWriter
    {
        private readonly RatesContext context;
        private readonly IMapper mapper;

        public RatesWriterEf(RatesContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        
        public async Task SetActualRates(ICollection<CurrencyRate> rates)
        {
            var actualRates = await context.ActualRates.ToListAsync();

            foreach (var newRate in rates)
            {
                var actualRate = actualRates.FirstOrDefault(r => r.Key == newRate.Key);
                if (actualRate == null)
                {
                    actualRate = newRate;
                    actualRates.Add(actualRate);
                    context.ActualRates.Add(actualRate);
                }
                else
                {
                    actualRate.Update(newRate, mapper);
                }
            }
        }

        public Task AddRatesToHistory(ICollection<CurrencyRate> rates)
        {
            context.RatesHistory.AddRange(rates.Select(r => r.ToHistoryItem(mapper)));

            return Task.CompletedTask;
        }

        public async Task SaveChanges()
        {
            await context.SaveChangesAsync();
        }
    }
}