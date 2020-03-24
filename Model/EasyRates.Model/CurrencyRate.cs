using System;

namespace EasyRates.Model
{
    public class CurrencyRate
    {
        
        public string CurrencyFrom { get; set; }
        
        public string CurrencyTo { get; set; }

        public string Key => CurrencyFrom + CurrencyTo;
        
        public decimal Value { get; set; }
        
        public DateTime TimeOfReceipt { get; set; }
        
        public DateTime ExpirationTime { get; set; }
        
        public DateTime OriginalPublishedTime { get; set; }
        
        public string ProviderName { get; set; }
    }
}