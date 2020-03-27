namespace EasyRates.RatesProvider.InMemory
{
    public class RatesProviderInMemoryOptions
    {
        public int Priority { get; set; } = 1;

        public string Name { get; set; } = "InMemory";
    }
}