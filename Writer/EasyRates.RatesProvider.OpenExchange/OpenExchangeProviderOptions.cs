namespace EasyRates.RatesProvider.OpenExchange
{
    public class OpenExchangeProviderOptions
    {
        public int Priority { get; set; }
        
        public string Name { get; set; }
        
        public string[] Currencies { get; set; }
    }
}