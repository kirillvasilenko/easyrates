using System.Threading.Tasks;
using EasyRates.Model;

namespace EasyRates.Reader.Model
{
    public interface IRatesReaderRepo
    {
        Task<CurrencyRate> GetRate(string from, string to);
        
        Task<CurrencyRate[]> GetRates(string from);
    }
}