using System;
using System.Linq;
using AutoFixture;
using EasyRates.Model;
using EasyRates.WriterApp;
using FluentAssertions;
using Moq;
using Xunit;

namespace EasyRates.Writer.Tests.WriterApp
{
    public class CurrencyRateProcessorTests
    {
        private readonly Fixture fixture = new Fixture();
        
        private readonly CurrencyRateProcessor processor;

        private readonly Mock<ICurrencyNameFormatter> formatter = new Mock<ICurrencyNameFormatter>();
        
        private readonly Mock<ITimetable> timetable = new Mock<ITimetable>();
        
        public CurrencyRateProcessorTests()
        {
            processor = new CurrencyRateProcessor(formatter.Object, timetable.Object);
        }

        
        [Fact]
        public void Process_FormatsNames()
        {
            var rates = fixture.CreateMany<CurrencyRate>().ToArray();

            var froms = rates.Select(r => r.CurrencyFrom).ToArray();
            var tos = rates.Select(r => r.CurrencyTo).ToArray();

            var expectedFroms = froms.Select(TurboFormat).ToArray();
            var expectedTos = tos.Select(TurboFormat).ToArray();
            
            foreach (var rate in rates)
            {
                formatter.Setup(f => f.FormatName(rate.CurrencyFrom)).Returns(TurboFormat(rate.CurrencyFrom));
                formatter.Setup(f => f.FormatName(rate.CurrencyTo)).Returns(TurboFormat(rate.CurrencyTo));
            }
            
            processor.Process(rates);

            var resultFroms = rates.Select(r => r.CurrencyFrom).ToArray();
            var resultTos = rates.Select(r => r.CurrencyTo).ToArray();

            resultFroms.Should().BeEquivalentTo(expectedFroms, c => c.WithStrictOrdering());
            resultTos.Should().BeEquivalentTo(expectedTos, c => c.WithStrictOrdering());
        }

        [Fact]
        public void Process_SetsExpirationTime()
        {
            var rates = fixture.CreateMany<CurrencyRate>().ToArray();

            var expectedExpirationTimes = rates.Select(r => TurboNextTime(r.TimeOfReceipt) + TimeSpan.FromSeconds(10)).ToArray();
            
            foreach (var rate in rates)
            {
                timetable.Setup(t => t.GetNextMoment(rate.TimeOfReceipt)).Returns(TurboNextTime(rate.TimeOfReceipt));
            }
            
            processor.Process(rates);

            var resultExpirationTimes = rates.Select(r => r.ExpirationTime).ToArray();

            resultExpirationTimes.Should().BeEquivalentTo(expectedExpirationTimes, c => c.WithStrictOrdering());

        }

        private DateTime TurboNextTime(DateTime time)
        {
            return time + TimeSpan.FromHours(1);
        }
        
        private string TurboFormat(string text)
        {
            return text + "Mickey";
        }
    }
}