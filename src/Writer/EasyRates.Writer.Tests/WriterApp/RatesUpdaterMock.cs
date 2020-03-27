using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EasyRates.WriterApp;

namespace EasyRates.Writer.Tests.WriterApp
{
    public class RatesUpdaterMock : IRatesUpdater
    {
        public int InvokesCount { get; set; }

        private Queue<InvokeCase> invokeCases;

        public RatesUpdaterMock()
        {
            invokeCases = new Queue<InvokeCase>();
        }
        
        public RatesUpdaterMock(ICollection<InvokeCase> cases)
        {
            invokeCases = new Queue<InvokeCase>(cases);
        }
        
        public async Task UpdateRates(CancellationToken ct)
        {
            if (invokeCases.TryDequeue(out InvokeCase currentCase))
            {
                currentCase = new InvokeCase();
            }

            InvokesCount++;
            
            await Task.Delay(currentCase.WorkTime, ct);
            if (currentCase.ThrowException)
            {
                throw new Exception();
            }
        }

        public class InvokeCase
        {
            public TimeSpan WorkTime { get; set; } = TimeSpan.FromMilliseconds(1);
            
            public bool ThrowException { get; set; }
        }
    }
}