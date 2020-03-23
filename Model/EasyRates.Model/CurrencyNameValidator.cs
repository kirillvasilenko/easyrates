namespace EasyRates.Model
{
    public class CurrencyNameValidator:ICurrencyNameValidator
    {
        public void Validate(string currencyName)
        {
            if (currencyName.Length == 3)
            {
                return;
            }
            throw new InvalidCurrencyNameException(currencyName);
        }
    }
}