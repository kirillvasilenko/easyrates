using System;

namespace EasyRates.Model
{
    public class CurrencyRateHistoryItem
    {
        public long Id { get; set; }
        
        public string From { get; set; }
        
        public string To { get; set; }
        
        public decimal Value { get; set; }
        
        public DateTime ExpirationTime { get; set; }
        
        public DateTime OriginalPublishedTime { get; set; }
        
        public DateTime TimeOfReceipt { get; set; }
        
        public string ProviderName { get; set; }
    }
}