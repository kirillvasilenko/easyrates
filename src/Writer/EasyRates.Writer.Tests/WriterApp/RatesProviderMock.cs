using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyRates.Model;

namespace EasyRates.Writer.Tests.WriterApp
{
    public class RatesProviderMock : IRatesProvider
    {
        public RatesProviderMock(
            string name,
            int priority,
            InvokeCase invokeCase)
        {
            ActualInvokeCase  = invokeCase;
            Name = name;
            Priority = priority;
        }
        
        public InvokeCase ActualInvokeCase { get; }
        
        public int Priority { get; }
        
        public string Name { get; }
        
        public bool Invoked { get; private set; }
        
        public async Task<CurrencyRate[]> GetAllRates()
        {
            Invoked = true;
            await Task.Delay(ActualInvokeCase.EmulationTime);
            if (ActualInvokeCase.ThrowException)
            {
                throw new Exception();
            }
            return ActualInvokeCase.Response;
        }

        public class InvokeCase
        {
            public TimeSpan EmulationTime { get; set; }
        
            public bool ThrowException { get; set; }
        
            public CurrencyRate[] Response { get; set; }
        }

        
    }
}