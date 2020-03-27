using System;

namespace EasyRates.RatesProvider.OpenExchange
{
    public class ErrorOnResponseToOpenExchangeException : Exception
    {
        public ErrorOnResponseToOpenExchangeException(
            int status,
            string message,
            string description,
            string request) : base(
            $"Get error on communication with OpenExchange.\n" +
            $"Request: {request}\n" +
            $"Status: {status}\n" +
            $"Message: {message}\n" +
            $"Description: {description}")
        {
            
        }
    }
}