using System;
using System.Threading.Tasks;
using ErrorMessages;
using Microsoft.Extensions.Logging;

namespace EasyRates.ErrorMessages.InMemory
{
    public class ErrorMessagesInMemory : IErrorMessages
    {
        private readonly ILogger<ErrorMessagesInMemory> logger;

        public ErrorMessagesInMemory(ILogger<ErrorMessagesInMemory> logger)
        {
            this.logger = logger;
        }
        
        public Task<string> GetMessage(string code, string defaultMessage, params object[] args)
        {
            try
            {
                string message;
                switch (code)
                {
                    case ErrorCode.RateNotFound:
                        message = "Курс {0} -> {1} не найден.";
                        break;
                    case ErrorCode.NoOneRateFound:
                        message = "Ни один курс для валюты {0} не найден.";
                        break;
                    case ErrorCode.InvalidCurrencyName:
                        message = "{0} - неправильный формат валюты. Название валюты должно состоять из 3 символов.";
                        break;
                    case ErrorCode.InternalServerError:
                        message = "На сервере произошла ошибка. Мы уже работаем над исправлением.";
                        break;
                    default:
                        message = null;
                        break;
                }
                
                var result = string.IsNullOrEmpty(message)
                    ? defaultMessage
                    :string.Format(message, args);
                return Task.FromResult(result);
            }
            catch (Exception e)
            {
                logger.LogWarning(e, e.Message);
                return Task.FromResult(defaultMessage);
            }
        }
    }
}