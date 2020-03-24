namespace EasyRates.RatesProvider.InMemory
{
    public class RatesProviderInMemoryOptions
    {
        public int Priority { get; set; } = 100;

        public string Name { get; set; } = "InMemory";
    }
}