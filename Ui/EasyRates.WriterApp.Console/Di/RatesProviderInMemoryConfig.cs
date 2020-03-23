namespace EasyRates.WriterApp.Console.Di
{
    public class RatesProviderInMemoryConfig
    {
        public int Priority { get; set; } = 10;

        public string Name { get; set; } = "InMemoryRatesProvider";

        public bool Enabled { get; set; }

    }
}