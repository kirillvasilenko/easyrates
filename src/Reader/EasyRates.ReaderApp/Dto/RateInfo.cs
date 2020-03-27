using System;

namespace EasyRates.ReaderApp.Dto
{
    /// <summary>
    /// Requested rate information
    /// </summary>
    public class RateInfo
    {
        /// <summary>
        /// Currency to
        /// </summary>
        public string CurrencyTo { get; set; }
    
        /// <summary>
        /// Rate value
        /// </summary>
        public decimal Rate { get; set; }
    
        /// <summary>
        /// Expiration time
        /// </summary>
        public DateTime ExpireAt { get; set; }
    }
}