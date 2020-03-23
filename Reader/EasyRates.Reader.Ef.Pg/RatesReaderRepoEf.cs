using System.Linq;
using System.Threading.Tasks;
using EasyRates.Model;
using EasyRates.Model.Ef.Pg;
using EasyRates.Reader.Model;
using Microsoft.EntityFrameworkCore;

namespace EasyRates.Reader.Ef.Pg
{
    public class RatesReaderRepoEf:IRatesReaderRepo
    {
        private readonly RatesContext context;

        public RatesReaderRepoEf(RatesContext context)
        {
            this.context = context;
        }
        
        public async Task<CurrencyRate> GetRate(string from, string to)
        {
            var result = await context.ActualRates.FirstOrDefaultAsync(
                d => d.From == from && d.To == to);

            if (result == null)
            {
                throw new RateNotFoundException(from, to);
            }

            return result;
        }

        public async Task<CurrencyRate[]> GetRates(string from)
        {
            var result = await context.ActualRates.Where(
                    d => d.From == from)
                .ToArrayAsync();

            if (!result.Any())
            {
                throw new NoOneRateFoundException(from);
            }

            return result;
        }
    }
}