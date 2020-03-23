namespace EasyRates.WriterApp.Console.Di
{
    public class RatesProviderOpenExchangeConfig
    {
        public bool Enabled { get; set; } = true;

        public bool ForPoorMode { get; set; } = true;
        
        public int Priority { get; set; } = 1;

        public string Name { get; set; } = "OpenExchangeProvider";

        public string AppId { get; set; }

        public string[] Currencies { get; set; } = {"USD"};

    }
}