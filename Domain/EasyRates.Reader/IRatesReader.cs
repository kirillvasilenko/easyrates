using System.Collections.Generic;
using System.Threading.Tasks;
using EasyRates.Model;

namespace EasyRates.Reader.Model
{
    public interface IRatesReader
    {
        Task<CurrencyRate> GetRate(string from, string to);
        
        Task<ICollection<CurrencyRate>> GetRates(string from);
    }
}