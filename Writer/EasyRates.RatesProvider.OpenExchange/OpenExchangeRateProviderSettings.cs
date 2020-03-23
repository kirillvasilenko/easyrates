namespace EasyRates.RatesProvider.OpenExchange
{
    public class OpenExchangeRateProviderSettings : IOpenExchangeRateProviderSettings
    {
        public int Priority { get; set; }
        
        public string Name { get; set; }
        
        public string[] Currencies { get; set; }
    }
}