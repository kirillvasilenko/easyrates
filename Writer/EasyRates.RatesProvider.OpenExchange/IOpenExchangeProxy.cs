using System.Threading.Tasks;

namespace EasyRates.RatesProvider.OpenExchange
{
    public interface IOpenExchangeProxy
    {
        Task<ActualRateResponse> GetCurrentRates(string from);
    }
}