using AutoFixture;
using FluentAssertions;
using Xunit;

namespace EasyRates.Model.Tests
{
    public class CurrencyRateTests
    {
        private Fixture fixture = new Fixture();
        
        [Fact]
        public void KeyShouldBeConcatFromAndTo()
        {
            var rate = new CurrencyRate()
            {
                CurrencyFrom = fixture.Create<string>(),
                CurrencyTo = fixture.Create<string>()
            };

            rate.Key.Should().Be(rate.CurrencyFrom + rate.CurrencyTo);
        }
    }
}