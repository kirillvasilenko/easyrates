namespace EasyRates.ReaderApp.Dto
{
    /// <summary>
    /// Response for rates request
    /// </summary>
    public class RatesResponse
    {
        /// <summary>
        /// Currency from
        /// </summary>
        public string CurrencyFrom { get; set; }
        
        /// <summary>
        /// Rates
        /// </summary>
        public RateInfo[] Rates { get; set; }
    }
}