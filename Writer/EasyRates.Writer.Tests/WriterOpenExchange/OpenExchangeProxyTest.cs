using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyRates.RatesProvider.OpenExchange;

namespace EasyRates.Writer.Tests.WriterOpenExchange
{
    public class OpenExchangeProxyTest : IOpenExchangeProxy
    {
        
        private readonly Dictionary<string, InvokeCase> caseByFrom;

        public OpenExchangeProxyTest(
            ICollection<InvokeCase> invokeCases)
        {
            caseByFrom = invokeCases.ToDictionary(t => t.From, t => t);
        }
        
        public async Task<ActualRateResponse> GetCurrentRates(string @from)
        {
            var invokeCase = caseByFrom[from];
            await Task.Delay(invokeCase.EmulationTime);
            if (invokeCase.ThrowException)
            {
                throw new Exception();
            }
            return invokeCase.Response;
        }
        
        public class InvokeCase
        {
            public string From { get; set; }
        
            public TimeSpan EmulationTime { get; set; }
        
            public bool ThrowException { get; set; }
        
            public ActualRateResponse Response { get; set; }
        }
    }
}