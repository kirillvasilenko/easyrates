namespace EasyRates.RatesProvider.OpenExchange
{
    public class OpenExchangeProviderOptions
    {
        public int Priority { get; set; } = 100;

        public string Name { get; set; } = "OpenExchange";
        
        public string[] Currencies { get; set; }
    }
}