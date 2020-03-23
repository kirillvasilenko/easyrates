using System.Threading.Tasks;

namespace EasyRates.RatesProvider.OpenExchange
{
    public interface IOpenExchangeProxy
    {
        Task<LatestRateResponse> GetCurrentRates(string from);
    }
}