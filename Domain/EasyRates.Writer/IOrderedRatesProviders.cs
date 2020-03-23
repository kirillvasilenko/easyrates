using System.Collections.Generic;

namespace EasyRates.Writer
{
    public interface IOrderedRatesProviders
    {
        ICollection<IRatesProvider> GetProviders();
    }
}