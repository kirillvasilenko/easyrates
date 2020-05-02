using System.Linq;
using System.Threading.Tasks;
using EasyRates.Model;
using EasyRates.Model.Spanner;
using EasyRates.Reader.Model;

namespace EasyRates.Reader.Spanner
{
    public class RatesReaderRepoEfSpanner:IRatesReaderRepo
    {
        private readonly RatesRepoSpanner repoSpanner;

        public RatesReaderRepoEfSpanner(RatesRepoSpanner repoSpanner)
        {
            this.repoSpanner = repoSpanner;
        }
        
        public async Task<CurrencyRate> GetRate(string from, string to)
        {
            var result = await repoSpanner.GetRate(from, to);

            if (result == null)
            {
                throw new RateNotFoundException(from, to);
            }

            return result;
        }

        public async Task<CurrencyRate[]> GetRates(string from)
        {
            var result = await repoSpanner.GetRates(from);
            if (!result.Any())
            {
                throw new NoOneRateFoundException(from);
            }

            return result;
        }
    }
}