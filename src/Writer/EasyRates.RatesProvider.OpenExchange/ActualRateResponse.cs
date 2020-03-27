using System;
using System.Collections.Generic;

namespace EasyRates.RatesProvider.OpenExchange
{
    public class ActualRateResponse
    {
        public long TimeStamp { get; set; }
        
        public string Base { get; set; }

        public Dictionary<string, decimal> Rates { get; set; }
        
    }
}