using System.Threading.Tasks;
using EasyRates.ReaderApp.Dto;

namespace EasyRates.ReaderApp
{
    public interface IReaderRateService
    {
        Task<RatesResponse> GetRate(string from, string to);
        
        Task<RatesResponse> GetRates(string from);
    }
}