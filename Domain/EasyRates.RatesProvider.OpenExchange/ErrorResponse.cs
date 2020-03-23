namespace EasyRates.RatesProvider.OpenExchange
{
    public class ErrorResponse
    {
        public int Status { get; set; }
        
        public string Message { get; set; }
        
        public string Description { get; set; }
    }
}