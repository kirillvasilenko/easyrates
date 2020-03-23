using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyRates.Model;

namespace EasyRates.Writer
{
    public interface IRatesWriter
    {
        Task SetActualRates(ICollection<CurrencyRate> rates);
        
        Task AddRatesToHistory(ICollection<CurrencyRate> rates);

        Task SaveChanges();
    }
}