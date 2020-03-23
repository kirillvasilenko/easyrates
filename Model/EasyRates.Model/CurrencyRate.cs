using System;

namespace EasyRates.Model
{
    public class CurrencyRate
    {
        
        public string From { get; set; }
        
        public string To { get; set; }

        public string Key => From + To;
        
        public decimal Value { get; set; }
        
        public DateTime TimeOfReceipt { get; set; }
        
        public DateTime ExpirationTime { get; set; }
        
        public DateTime OriginalPublishedTime { get; set; }
        
        public string ProviderName { get; set; }
    }
}