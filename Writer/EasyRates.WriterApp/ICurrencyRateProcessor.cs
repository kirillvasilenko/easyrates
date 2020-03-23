using System.Collections.Generic;
using EasyRates.Model;

namespace EasyRates.WriterApp
{
    public interface ICurrencyRateProcessor
    {
        void Process(ICollection<CurrencyRate> rates);
    }
}