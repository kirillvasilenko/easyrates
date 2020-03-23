using System;
using System.Collections.Generic;
using Time;

namespace EasyRates.RatesProvider.OpenExchange
{
    public class LatestRateResponse
    {
        public long TimeStamp { get; set; }
        
        public DateTime TimeStampAsTime => TimeStamp.ToDateTimeUtc();
        
        public string Base { get; set; }

        public Dictionary<string, decimal> Rates { get; set; }
        
    }
}