using AutoFixture;
using EasyRates.RatesProvider.OpenExchange;
using FluentAssertions;
using Xunit;

namespace EasyRates.Tests.Domain.RatesProviderOpenExchange
{
    public class Converters
    {
        private Fixture fixture = new Fixture();

        [Fact]
        public void LatestRateResponseToCurrencyRates()
        {
            var providerName = fixture.Create<string>();
            var response = fixture.Create<LatestRateResponse>();

            var rates = response.ToDomain(providerName);

            rates.Should().HaveCount(response.Rates.Count);
            foreach (var rate in rates)
            {
                rate.From.Should().Be(response.Base);
                var rateValue = response.Rates[rate.To];
                rate.Value.Should().Be(rateValue);
                rate.ProviderName.Should().Be(providerName);
            }
            
        }
    }
}