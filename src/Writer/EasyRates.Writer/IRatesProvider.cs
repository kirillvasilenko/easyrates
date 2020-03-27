using System.Collections.Generic;
using System.Threading.Tasks;
using EasyRates.Model;

namespace EasyRates.Writer
{
    public interface IRatesProvider
    {
        int Priority { get; }
        
        string Name { get; }

        Task<CurrencyRate[]> GetAllRates();
    }
}