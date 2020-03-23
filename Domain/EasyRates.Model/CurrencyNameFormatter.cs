namespace EasyRates.Model
{
    public class CurrencyNameFormatter:ICurrencyNameFormatter
    {
        public string FormatName(string currencyName)
        {
            return currencyName.ToUpper();
        }
    }
}