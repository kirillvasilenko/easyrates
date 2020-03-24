using System;
using AutoFixture;
using EasyRates.RatesProvider.OpenExchange;
using FluentAssertions;
using Xunit;

namespace EasyRates.Writer.Tests.WriterOpenExchange
{
    public class Converters
    {
        private readonly Fixture fixture = new Fixture();

        [Fact]
        public void ActualRateResponseToCurrencyRates()
        {
            var providerName = fixture.Create<string>();
            var currentTime = DateTime.UtcNow;
            var response = fixture.Create<ActualRateResponse>();

            var rates = response.ToDomain(providerName, currentTime);

            rates.Should().HaveCount(response.Rates.Count);
            foreach (var rate in rates)
            {
                rate.From.Should().Be(response.Base);
                var rateValue = response.Rates[rate.To];
                rate.Value.Should().Be(rateValue);
                rate.ProviderName.Should().Be(providerName);
                rate.TimeOfReceipt.Should().Be(currentTime);
                rate.OriginalPublishedTime.Should().Be(response.TimeStamp.ToDateTimeUtc());
            }
            
        }
    }
}