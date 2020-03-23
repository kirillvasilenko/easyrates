namespace EasyRates.RatesProvider.OpenExchange
{
    public interface IOpenExchangeRateProviderSettings
    {
        int Priority { get; }
        
        string Name { get; }
        
        string[] Currencies { get; }
    }
}