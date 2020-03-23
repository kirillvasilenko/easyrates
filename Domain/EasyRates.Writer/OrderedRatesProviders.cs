using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyRates.Writer
{
    public class OrderedRatesProviders : IOrderedRatesProviders
    {
        private readonly IRatesProvider[] ratesProviders;
        
        public OrderedRatesProviders(IEnumerable<IRatesProvider> ratesProviders)
        {
            this.ratesProviders = ratesProviders.ToArray();
            Array.Sort(this.ratesProviders, 
                (p1, p2) => p2.Priority.CompareTo(p1.Priority));
        }
        
        public ICollection<IRatesProvider> GetProviders()
        {
            return ratesProviders;
        }
    }
}