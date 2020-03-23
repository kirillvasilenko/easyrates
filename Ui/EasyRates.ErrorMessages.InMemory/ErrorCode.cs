using System;
using ErrorMessages;

namespace EasyRates.ErrorMessages.InMemory
{
    
    public static class ErrorCode
    {
        public const string InvalidCurrencyName = "InvalidCurrencyName";

        public const string RateNotFound = "RateNotFound";
        
        public const string NoOneRateFound = "NoOneRateFound";
        
        public const string InternalServerError = CommonErrorCode.InternalServerError;
        
    }
}