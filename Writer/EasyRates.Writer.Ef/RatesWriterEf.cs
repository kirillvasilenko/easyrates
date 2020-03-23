using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyRates.Model;
using EasyRates.Model.Ef;
using Microsoft.EntityFrameworkCore;
using Time;

namespace EasyRates.Writer.Ef
{
    public class RatesWriterEf:IRatesWriter
    {
        private readonly RatesContext context;
        
        public RatesWriterEf(RatesContext context)
        {
            this.context = context;
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
                    actualRate.Update(newRate);
                }
            }
        }

        public Task AddRatesToHistory(ICollection<CurrencyRate> rates)
        {
            context.RatesHistory.AddRange(rates.Select(r => r.ToHistoryItem()));

            return Task.CompletedTask;
        }

        public async Task SaveChanges()
        {
            await context.SaveChangesAsync();
        }
    }
}