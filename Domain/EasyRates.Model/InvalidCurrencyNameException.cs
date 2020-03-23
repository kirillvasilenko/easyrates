using System;

namespace EasyRates.Model
{
    public class InvalidCurrencyNameException:Exception
    {
        public string CurrencyName { get; }

        public InvalidCurrencyNameException(string currencyName) : base(
            $"Currency name {currencyName} is an invalid currency name. It must have 3 symbols.")
        {
            CurrencyName = currencyName;
        }
    }
}